# Deploying container to an Azure Container Instance

This lab will walk through creating an ASP.Net Core application as container and debugging and then finally deploying the container to the Azure Container Instance.

In this lab you will build the app and create the container on your Windows or Mac machine.
 
Make sure you have installed the following pre-requisites on your machine

| Prerequisites        |            | 
| ------------- |:-------------| 
| .NET Core 2.0    | [Install](https://www.microsoft.com/net/download/core) | 
| Docker     | Download and install: [Docker Windows](https://download.docker.com/win/stable/InstallDocker.msi) - [Docker Mac](https://download.docker.com/mac/stable/Docker.dmg)| 
| VS Code with C# Plugin    | [Install](https://code.visualstudio.com/Download)      | 
| Node.js   |  [Install](https://nodejs.org/en/download/)   |
| Yeoman   | Run from command prompt: **npm install -g yo** |
| Yeoman generator for Docker  | Run from command prompt: **npm install -g generator-docker**  
||On Windows run from the folder "Program Files/nodejs/node_modules" |
| Bower-A package manager for the web | Run from command prompt: **npm install -g bower** |
| Gulp | Run from command prompt: **npm install -g gulp** |

## Task 1: Create ASP.NET Core Hello World Application
1. Open a command prompt and change your directory to your code location. Then run following commands to create an ASP.Net Core app.
    ```
    md helloworld
    cd helloworld
    dotnet new  web
    dotnet restore
    code .
    ```
2. Open *Program.cs*

    Add missing assets  necessary to debug the application by selecting **Yes** on the notification bar.
    
    Add the following line of code before the line ```.Build();```
    ```c-sharp
    .UseUrls("http://*:8080")
    
    ```
    Example:
    ```
    public static IWebHost BuildWebHost(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
        .UseStartup<Startup>()
        .UseUrls("http://*:8080")
        .Build();
    ```
3. Press **Ctrl+S** to save the changes.

4. Open *Startup.cs* and modify the following line of code to match what is shown in the Configure method:
    ```
    await context.Response.WriteAsync($"Hello World v1! {DateTime.Now}");
    ```
5. Press **Ctrl-S** to save changes.

6. Press **F5** and confirm the app runs correctly by browsing to http://localhost:8080.

7. Stop the run before proceeding to Task 2.

## Task 2: Create a Docker Image
To support different developer environments, the .NET Core application will be deployed to Linux.  Modify the application you started in Task 1 as follows:

1. Open the VS Code Terminal windows by pressing **Ctrl-`**.   Publish app using VS Code terminal window with the following command: 
    ```
    dotnet restore
    dotnet publish
    ```

3. Create new file by clicking [**Ctrl-N**] in VS Code and re-name it to *dockerfile*. You can do this by first right-click->Save As->Right Click->Rename. Ensure that there is no extension. The file name should be simply 'dockerfile'.

4. Add the following.  Make sure to replace **\<appname>** in **ENTRYPOINT** last line to match your application name. For example helloworld.dll
    ```
    FROM microsoft/aspnetcore:2.0
    RUN apt-get update
    RUN apt-get install -y curl unzip 
    RUN curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l ~/vsdbg
    WORKDIR /app
    COPY ./bin/Debug/netcoreapp2.0/publish .
    EXPOSE 8080/tcp
    ENV ASPNETCORE_URLS http://*:8080
    ENTRYPOINT ["dotnet", "/app/helloworld.dll"]
    ```
    
5. Press **Ctrl-S** to save changes.

6. In the Terminal windows, create the Docker image by running the following commands.  Ensure to substitute <your_image_name> with for example helloworldfromlinux :
    ```
    docker build -t <your_image_name> .
    docker run -d -p 8080:8080  <your_image_name>
    docker ps
    ```
    
7. Browse to http://localhost:8080 or IP address listed in Container list (Mac)

8. Close browser

9. In VS Code, update *startup.cs*
    ```
    await context.Response.WriteAsync($"Hello World v2! {DateTime.Now}");
    
    ```
10. Press **Ctrl-S** to save changes.

11. Run the following commands to publish the updated application, create an updated image, and run the image in a new container.
    ```
    dotnet publish
    docker build -t <your_image_name> .
    docker run -d -p 8080:8080  <your_image_name>
    ```
12. Browse to http://localhost:8080 or IP address listed in Container list (Mac)

13. Close browser




## Task 3: Debug an Application in a Docker Container

**Note: This section is ungoing an update and may not work correctly at this time**

1. Publish debug version of the application
    ```none
    dotnet publish -c Debug 
    ```
2. Build a new version of the image
    ```none
    docker build -t <your_image_name> --rm .
    ```
3. Find old version and kill
    ```none
    docker ps
    docker kill <image_id>
    ```
4. Start debug version
    ```none
    docker run -d -p 8080:8080 --name debug <your_image_name>
    ```
5. Add the following as the last json object in the *configurations* array in the *launch.json* (found in  the *.vscode* folder). Make sure to update the *"pipesArgs"* value and the *"sourceFileMap* values accordingly.
    ```
    {
                "name": ".NET Core Remote Attach",
                "type": "coreclr",
                "request": "attach",
                "processId": "${command:pickRemoteProcess}",
                "pipeTransport": {
                    "pipeProgram": "powershell.exe",
                    "pipeArgs": [ "-c", "docker exec -i debug ${debuggerCommand}" ],
                    "debuggerPath": "/root/vsdbg/vsdbg",
                    "pipeCwd": "${workspaceRoot}",
                    "quoteArgs": true
                },
                "sourceFileMap": {
                    "<path to your application's root directory>": "${workspaceRoot}"
                },
                "justMyCode": true
    }
    ```


6. In VS Code, click on Debug icon in the left toolbar *insert image*

7. From **Debug** dropdown, select **.NET Core Remote Attach**

8. Go to the *Startup.cs* file.  Set a breakpoint on the line by clicking in the left-hand side gutter
    ```c-sharp
    await context.Response.WriteAsync($"Hello World v2! {DateTime.Now}");
    ```
     

9. Press **F5** or click Debug Run Icon *insert image*

10. Hit the breakpoint.  Press **F5** to complete the request.


## Task 4: Deploy to Azure Container Instance

1. Open browser and go to [http://portal.azure.com](http://portal.azure.com)

2. Login using your Azure credentials

3. Click on the Cloud Shell icon at the top of the page.   The shell may take some time to be initialized.


4. In the terminal window, create an Azure Resource Group to hold your workshop resources.  You can delete the is Resource Group at the end of the workshop.  Deleting the resource group will delete all of the assets created.
 
5. Enter the following command:
    ```none
    az group create --name <your_group_name> --location eastus
    ```
    
5. Create an Azure Container Registry.  The ACR is used to store Docker images in the cloud.  Azure Container Instances will pull images from this registry when deploying a new container.
    ```
    az acr create --resource-group <your_group_name> --name <registry_name> --sku Basic --admin-enabled true
    ```
6. You will need information about the registry to use when creating containers in ACI.  In the response from the **az acr create** command, find the value for *"loginServer"*.  This value will be referred to as *<reg_login_server>* in later commands.

7. Get the password for teh registry using the following command:
    ```
    az acr credential show --name <registry_name> --query passwords[0].value
    ```

8. Copy and paste the password into a text document.  The password will be referred to as *<registry_password>* in later commands.

9. Return to VS Code.

10. In the Terminal windows, login to the Azure Container Registry using the following command:
    ```
    docker login --username=<registry_name> --password=<reg_password> <reg_login_server>
    ```
11. List your Docker images using the following command:
    ```
    docker images
    ```
12. We will tag the latest image as version one of our ACI image.  Run the follwing command:
    ```
    docker tag <image_name> <reg_login_server>/<image_name>:v1
    ```
13. Confirm that image was created with the following command:
    ```
    docker images
    ```
14. Now it is time to push the iamge to the Azure Container Registry.  Run the following command:
    ```
    docker push <reg_login_server>/<image_name>:v1
    ```
15. To return a list of images that have been pushed to your Azure Container registry, run the following command in the Cloud Shell in your browser:
    ```
    az acr repository list --name <registry_name> --username <registry_name> --password <registry_password> --output table
    ```
16. If you need to see tags that have been applied to a specific image, run the following command:
    ```
    az acr repository show-tags --name <registry_name> --username <registry_name> --password <registry_passwrod> --repository <image_name> --output table
    ```
17. It is now time to deploy our app.  To deploy your container image from the container registry with a resource request of 1 CPU core and 1GB of memory, run the following command:
    ```
    az container create --name <container_name> --image <reg_login_server>/<image_name>:v1 --cpu 1 --memory 1 --registry-login-server <reg_login_server> --registry-username <registry_name> --registry-password <registry_password> --ip-address public -g <resource_group_name> --port 8080
    ```

18. Within a few seconds, you should get a response to your request. Initially, the container will be in a Creating state, but it should start within a few seconds. You can check the status using the show command:
    ```none
    az container show --name <container_name> --resource-group <your_group_name>
    ```

 20. At the bottom of the output, you will see the container's provisioning state and its IP address:
    ```json
    
    "ipAddress": {
          "ip": "13.88.8.148",
          "ports": [
            {
              "port": 80,
              "protocol": "TCP"
            }
          ]
        },
        "osType": "Linux",
        "provisioningState": "Succeeded"

    ```
21. Once the container moves to the Succeeded state, you can reach it in your browser using the IP address provided.

22.  When finished, you can delete the container (and save money)
    ```none
    az container delete --name <container_name> --resource-group <your_group_name>
    ```

### Blog reference:
[Visual Studio, Docker and ACI Better Together!](https://blogs.msdn.microsoft.com/alimaz/2017/08/17/visual-studio-docker-azure-container-instances-better-together/)



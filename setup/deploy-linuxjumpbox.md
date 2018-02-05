# A Very Simple Deployment of a Linux VM through the Azure Portal #
We're going to use the Azure portal to deploy a jumpbox for the later exercises. 

## Create the Jumpbox
1. Log into the Azure Portal at https://portal.azure.com
2. Click on the `+` in the upper left, then click on `Compute`, then select Ubuntu 16.04, and on the next page, click `Create`.
3. On the basics page, fill in the following:
    1. **Name:**    *enter a name for the server* (ex. jumpbox) 
    1. **Username:**    *enter a username* (ex. adminuser)
    1. **Authentication Type:** choose `Password`
    2. **Password:**    *enter a password*
    3. **Resource Group:** pick one these two options:
         1. Create new: *enter a name for your resource group* (ex. jumpboxrg)
         2. Select existing: select the existing Resource Group from the list.
    4. **Location:** select Location for the server (ex. East US)

    Then click `OK`

4. On the next page, choose your machine size.  Since we're not doing anything fancy on this jumpbox, a DS1_V2 Standard is fine.  
Select the **DS1_V2 Standard** tile and click `Select`.

     _Note:_ If you do not see this size listed, click the `View All` link in the upper right section, just below the Minimum Memory textbox.
3. On the `Settings` page, just click `OK`.  
4. Finally, on the `Purchase` page, review the settings, then click `Purchase`

At this point, your linux VM will begin to deploy.   This may take a minute or two.

Once your VM has deployed, navigate to it in the portal.  Take a look at the Overview settings page.  Click on the `Connect` button at the top to get the ssh command to remotely log into your jumpbox.  It will look something like this:
        ssh [username]@[ip address]

> On Windows and need SSH? Try any of the following: 

>   [Cloud Shell](https://docs.microsoft.com/en-us/azure/cloud-shell/quickstart) |
>   [Putty](http://www.chiark.greenend.org.uk/~sgtatham/putty/download.html) |
>   [Bitvise SSH](https://www.bitvise.com/ssh-client-download) | 
>   [Windows Subsystem for Linux](https://msdn.microsoft.com/en-us/commandline/wsl/install_guide)


## Connect to the Jumpbox
From your laptop, ssh into the jumpbox using above of the above tools you have adapted for your environment.

## Install the Azure cli
1. First, you need to install some prereqs. Run below commands from your SSH bash command:
```
sudo apt-get update && sudo apt-get install -y libssl-dev libffi-dev python-dev build-essential
```
2. Next, install the cli
```
curl -L https://aka.ms/InstallAzureCli | bash
exec -l $SHELL
```

The cli is now installed!

Finally, configure the output to default to table format.  Run:
```
az configure
```
You wil then be prompted to change the configuration settings.  Type `y` and hit enter.
In the list the is displayed, enter the number for the choice `table` and hit enter.
You will be prompted to enable the log file.  Enter your choice and hit enter. 
You will be prompted to enable data collection to send to Microsoft.  Enter your choice and hit enter.
Your settings will be saved.

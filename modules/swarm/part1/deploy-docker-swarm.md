<!-- This section leverages the new, preview Swarm Mode (aka Docker CE) features
of ACS.  However, as of the date of this lab (9/25/17) these features are only availablea
using the cli/powershell/ARM; they are not available via the portal.
As soon as the feature is available in the portal, this module will be updated.
-->

# Deploy a Swarm Mode cluster
This lab will create a Docker Swarm Mode (Docker CE- which is Docker Community Edition) cluster using Azure Container Services.  This is a new, preview capability of Azure.

## Deploy the cluster
1. First, go back to your linux jumpbox and run the following command to generate ssh keys that will be used later in the exercise:  (leave the pasphrase blank)
```
ssh-keygen -t rsa -b 2048 -C "acsadmin@acs" -f ~/.ssh/acs_rsa
```
then:
```
cat ~/.ssh/acs_rsa.pub
```
> Alternatively, if you are comfortable, you can use your local SSH program (such as Bitvise or Putty) to generate the SSH keys.  Just adapt the instructions as necessary.

2. Now let's use the cli to create the Swarm-mode cluster.  First we need to create a resource group:
> Note: Please use the **westcentralus** region as this preview capability is not available yet in all Azure regions 
```
az group create --name SwarmRG --location westcentralus 
```

3. Next, let's create the cluster.  The following command creates a cluster named *mySwarmCluster* with one Linux master node and three Linux agent nodes.
```
az acs create --name mySwarmCluster --orchestrator-type dockerce --resource-group SwarmRG --ssh-key-value ~/.ssh/acs_rsa.pub
```
Once this command finishes, you can verify it executed by running

    az acs list
You should see output that looks like:
```
Location       Name            ProvisioningState    ResourceGroup
-------------  --------------  -------------------  ---------------
westcentralus  mySwarmCluster  Succeeded            SwarmRG
```

## Connect to the Cluster


Now that the cluster is deployed, we need to ssh to the master.  The first step is to find the DNS name that was assigned to the master.  We can find this using the cli:


    MASTERFQDN=$(az acs list -o tsv --query [].masterProfile.fqdn|grep swarmrg); echo $MASTERFQDN
    
Similarly, we need to find and save the DNS name for the agents in our cluster.  Run the following command:

    AGENTFQDN=$(az acs list -o tsv --query [].agentPoolProfiles[0].fqdn|grep swarmrg); echo $AGENTFQDN

Make note of this name (save it to a scratchpad); we'll refer to it later as AGENTFQDN.

At this point, we can ssh to your master using the following (which automatically substitutes the master name using the $MASTERFQDN shell variable):

    ssh -i ~/.ssh/acs_rsa azureuser@$MASTERFQDN

Now that you are on the swarm master, check the status of the cluster by running:

    docker info

Look for the following information within the resulting output:

    Swarm: active
     NodeID: 1i8hnn4v7msygj2z7nrk2p7zu
     Is Manager: true
     ClusterID: 78f3x9oea40piz6rai37srgdv
     Managers: 1
     Nodes: 3


## Begin managing the Swarm cluster

First, let's do a little work to configure our cluster for this lab.  Run the following:

    docker node update --availability active $(docker node ls -f "role=manager" -q)

> This command enables you to run containers on the master, which we'll need to do later.

Now the cluster is ready to go!

To view the nodes in your cluster:

    docker node ls

    ID                            HOSTNAME                      STATUS              AVAILABILITY        MANAGER STATUS
    9kexly729ylezsqc6pow6zws7     swarmm-agent-13957614000000   Ready               Active
    kc2sht405ewdmi2qxytsf7y0w     swarmm-agent-13957614000002   Ready               Active
    nzd4h33heoyobqumqmcn8snue *   swarmm-master-13957614-0      Ready               Active               Leader
    
## Deploy Nginx ##
To deploy an Nginx container run the following command:

    docker service create --replicas 1 --name my_web --publish 80:80 nginx

Check the service is running:

    docker service ls

    ID            NAME           REPLICAS  IMAGE   COMMAND
    atz0nm4nx9rg  my_web         1/1       nginx

See what node the container is running on:

    docker service ps my_web

Scale the service up to three nodes:

    docker service scale my_web=3

Inspect the details of the service. If you leave off the "pretty" switch, you'll get a response in JSON:

    docker service inspect --pretty my_web

Let's access the nginx web server from our browser.  To do that, we need to first 

From your browser on your laptop, browse to http://[AGENTFQDN]. You should see the nginx welcome screen.

## Graphically inspect the cluster

Let's use the Docker Visualizer to visually inspect our cluster. 
Run the following command to load Docker Visualizer:
```
docker service create \
  --name=viz \
  --publish=8080:8080/tcp \
  --constraint=node.role==manager \
  --mount=type=bind,src=/var/run/docker.sock,dst=/var/run/docker.sock \
  dockersamples/visualizer
  ```
Now in your browser, open a new tab and go to http://[AGENTFQDN]:8080
  

You should see a graphical vizualization of your cluster.  You can watch it scale your services up and down:

    docker service scale my_web=5
    [look at Docker Visualizer]
    docker service scale my_web=1
    [look at Docker Visualizer]
    docker service scale my_web=5
    [look at Docker Visualizer]

Now mark a node as unavailable, and watch Swarm re-allocate the containers:
    
    docker node ls
    [make a note of one of the agent node names]
    docker node update --availability=drain [agent node name]
    [look at docker visualizer ]
    docker node update --availability active [agent node name]

Finally, delete the service:

    docker service rm my_web

## Optional steps:  Monitor your cluster

Follow the steps outlined in section [Instrument & Monitor your containers](/modules/oms/oms4containers.md) to get your Workspace id and your secret key. Subsitute in below command accordingly:


On your swarm master, run the following to set up your secrets:
```
export WSID=<your workspace id>
export KEY=<your secret key>
echo $WSID | docker secret create WSID -
echo $KEY |  docker secret create KEY -
docker secret ls
```
Then run:
```
docker service create --name omsagent --mode global \
--mount type=bind,source=/var/run/docker.sock,destination=/var/run/docker.sock \
--secret source=WSID,target=WSID --secret source=KEY,target=KEY \
-p 25225:25225 -p 25224:25224/udp \
--restart-condition=on-failure microsoft/oms
 ```
 And in a few minutes, you should see your swarm appear in your OMS workspace.



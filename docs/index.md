# Container Camp #

The official un-official container camp used to build out containerized applications on Azure.

We assume you have an Azure Subscription... If you don't, break out your Microsoft Account (aka LiveID, Hotmail account, etc) and pick one of these options:

* [Free $200/One Month Trial](https://azure.microsoft.com/en-us/free/) – $200 credit for use in 30 days.
* [Visual Studio Dev Essentials Program](https://www.visualstudio.com/dev-essentials/?campaign=VSBlog_AzureXamAnnoucement_VSDE) – Comes with $25 a month of Azure credit for 12 months.
* [IT Pro Cloud Essentials Program](https://www.microsoft.com/itprocloudessentials/en-US) – Also comes with $25 a month of Azure credit for 12 months.


## Setup : Create an Azure Linux Jumpbox  ##
In this setup, you will create a linux jumpbox VM in Azure using the Azure Portal, install the Azure cli, and install docker on the vm.

- Setup Step 1: [Deploy a simple Linux VM jumpbox using portal](setup/deploy-linuxjumpbox.md)
- Setup Step 2: [Login to Azure CLI](setup/xplat-cli-login.md)
- Setup Step 3: [Install Docker on the jumpbox](setup/azdockerinstall.md)
- Setup Step 4: [Clone this github repository](setup/gitclone.md)


## Module : Deploy some containers on your jumpbox ##
Starting off with containers using a Linux VM and Docker

1. [Use the Jumpbox to deploy containers](modules/docker/deploy-docker-vm.md)
2. [Create a custom container](modules/docker/buildimage.md)
3. [Instrument & Monitor your containers](modules/oms/oms4containers.md)


## Module : Configure a Windows Container Host ##
Build a Windows 2016 Server Container Host and deploy Windows containers.

* [Windows Containers on Windows Server](modules/windowscontainers/windows-containers.md)


## Module : Setup Docker Swarm and Deploy Some Containers ##
Deploy Docker with swarm mode, using an acs-machine template to deploy to Azure. Once you have a swarm cluster you will deploy some things to it...

* [Deploy a Swarm Mode cluster](modules/swarm/part1/deploy-docker-swarm.md)


## Module : Deploy Multicontainer Applications
Experiment with using docker compose to deploy multi-container applications

* [Deploy multicontainer applications](modules/swarm/part2/multiapp.md)


## Module : Deploy Containers to Azure aks with Kubernetes

* [Deploy Containers to Azure aks with Kubernetes](modules/kubernetes/kubernetes.md)


## Module : Deploy an Azure Container Instance

* [Deploing container to an Azure Container Instance](modules/azurecontainerinstances/aci.md)


## Module : Securing Containerized Applications
Experiment with securing containerized application by scanning for vulnerabilities, locking down the runtime environment and enforcing compliance.

* [Securing Containerized Applications](modules/security/README.md)



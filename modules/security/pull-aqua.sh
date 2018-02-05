wget -O- https://get.aquasec.com/$1/aquasec-server-2.5.3.tar.gz | gunzip -c > aquasec-server-2.5.3.tar
wget -O- https://get.aquasec.com/$1/aquasec-gateway-2.5.3.tar.gz | gunzip -c > aquasec-gateway-2.5.3.tar
wget -O- https://get.aquasec.com/$1/aquasec-database-2.5.3.tar.gz | gunzip -c > aquasec-database-2.5.3.tar
wget -O- https://get.aquasec.com/$1/aquasec-scanner-cli-2.5.3.tar.gz | gunzip -c > aquasec-scanner-cli-2.5.3.tar
wget -O- https://get.aquasec.com/$1/aquasec-agent-2.5.3.tar.gz | gunzip -c > aquasec-agent-2.5.3.tar

docker load -i aquasec-server-2.5.3.tar
docker load -i aquasec-gateway-2.5.3.tar
docker load -i aquasec-database-2.5.3.tar
docker load -i aquasec-scanner-cli-2.5.3.tar

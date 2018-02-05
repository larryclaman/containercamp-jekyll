curl -u administrator:workshop --data '{}' -X POST http://$(hostname):80/api/v1/scanner/registry/Docker%20Hub/image/ravitella%2fdocker-springboot-recommendationservice:latest/scan
curl -u administrator:workshop --data '{}' -X POST http://$(hostname):80/api/v1/scanner/registry/Docker%20Hub/image/ravitella%2fdocker-springboot-readinglistapplication:latest/scan
curl -u administrator:workshop --data '{}' -X POST http://$(hostname):80/api/v1/scanner/registry/Docker%20Hub/image/mysql%2fmysql-server:latest/scan
docker pull ravitella/docker-springboot-recommendationservice:latest
docker pull ravitella/docker-springboot-readinglistapplication:latest
docker pull mysql/mysql-server:latest 

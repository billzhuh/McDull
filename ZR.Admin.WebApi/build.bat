@echo off

docker build -t ZRAdmin.NET:latest -f ./Dockerfile .

echo "==============�鿴����==========="
docker images

pause
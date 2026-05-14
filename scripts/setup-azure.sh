#!/bin/bash
RESOURCE_GROUP="rg-challengeapi"
LOCATION="brazilsouth"
VM_NAME="vm-challengeapi"
ADMIN_USER="clyvovet"

echo "== CLYVO VET - Provisionamento Azure =="

az group create --name "$RESOURCE_GROUP" --location "$LOCATION" --output table

az vm create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --image Ubuntu2204 \
  --size Standard_B2s \
  --admin-username "$ADMIN_USER" \
  --generate-ssh-keys \
  --output table

az vm open-port --resource-group "$RESOURCE_GROUP" --name "$VM_NAME" --port 8080 --priority 1001
az vm open-port --resource-group "$RESOURCE_GROUP" --name "$VM_NAME" --port 1521 --priority 1002
az vm open-port --resource-group "$RESOURCE_GROUP" --name "$VM_NAME" --port 22 --priority 1003

az vm run-command invoke \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --command-id RunShellScript \
  --scripts "
    apt-get update -y
    apt-get install -y git nano curl
    curl -fsSL https://get.docker.com | sh
    curl -L https://github.com/docker/compose/releases/download/v2.24.0/docker-compose-linux-x86_64 -o /usr/local/bin/docker-compose
    chmod +x /usr/local/bin/docker-compose
    useradd -m -s /bin/bash appuser
    usermod -aG docker appuser
    docker --version
    docker-compose --version
    echo 'Setup concluido!'
  "

IP=$(az vm list-ip-addresses --resource-group "$RESOURCE_GROUP" --name "$VM_NAME" \
  --query "[].virtualMachine.network.publicIpAddresses[0].ipAddress" --output tsv)

echo "== VM PRONTA! =="
echo "IP: $IP"
echo "SSH: ssh $ADMIN_USER@$IP"
echo "API: http://$IP:8080"
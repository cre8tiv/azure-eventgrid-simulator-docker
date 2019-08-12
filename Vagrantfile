# -*- mode: ruby -*-
# vi: set ft=ruby :

Vagrant.configure("2") do |config|
  config.vm.box = "bento/ubuntu-16.04"

  config.vm.provision "shell", path: "bootstrap.sh"
  config.vm.provision "shell", inline: "cd /vagrant && sudo docker-compose -f ./docker-compose.yml build", run: "always"
  config.vm.provision "shell", inline: "cd /vagrant && sudo docker-compose -f ./docker-compose.yml up -d", run: "always"
  config.vm.provision "shell", inline: "cd /vagrant && sudo docker-compose -f ./docker-compose.yml ps", run: "always"

  # for eventgrid
  config.vm.network "forwarded_port", guest: 60101, host: 60101
  config.vm.network "forwarded_port", guest: 60102, host: 60102  
 
  # for ssh
  config.vm.network "forwarded_port", id: "ssh", guest: 22, host: 60022

  config.vm.provider "virtualbox" do |vb|
    # Display the VirtualBox GUI when booting the machine
    vb.gui = false

    # Customize the amount of memory on the VM
    vb.memory = "4096"
    vb.cpus = "2"
  end
end

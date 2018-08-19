#Requires -RunAsAdministrator

# https://github.com/praeclarum/Ooui/issues/100

netsh http add urlacl url=http://*:6060/ user=\Everyone listen=yes

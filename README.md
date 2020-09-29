# Nanny

Nanny - is a command line interface for creation Github Pull Requests for projects with multiple release versions.

![Master status](https://api.travis-ci.com/kolenkainc/nanny.svg?branch=master "master status")
[![codecov](https://codecov.io/gh/kolenkainc/nanny/branch/master/graph/badge.svg?token=32CKSF3ZS6)](codecov)

## Installation
### Linux
```bash
wget -qO - https://raw.githubusercontent.com/kolenkainc/Nanny.Releases/master/PUBLIC.KEY | sudo apt-key add -
echo "deb https://raw.githubusercontent.com/kolenkainc/Nanny.Releases/master/ focal main" | sudo tee /etc/apt/sources.list.d/kolenka.list
sudo apt-get update
sudo apt-get install nanny
```
### Macos
```bash
brew tap kolenkainc/nanny
brew install nanny
```

## Pre-requisites
* You must install and configure Gitbash

## Step 01: Create yout Gihhub repository
Create a new repository on github and copy its link:

``
https://github.com/yourAccount/test01.git
``

## Step 02: Initialize your repository

* Create a gitignore file for the repository
* Open the game repository
* Launch Gitbash on the game installation directory
* (Gitbash) Add a readme file 
```
touch README.md
echo "MY GIT REPOSITORY" >> README.md
```

* Initialize the repository with the gitignore:
```
git init
git remote add origin https://github.com/yourAccount/test01.git
git remote add origin
git add .gitignore
git commit -m ".gitignore"
git push --set-upstream origin master
git push
```

* Now, add the rest of the repository:

```
git rm -rf --cached 
git add .
git commit "first commit"
git push
```


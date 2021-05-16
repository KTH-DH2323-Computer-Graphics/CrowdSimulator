# DH2323 - Project

## [Live Demo](https://kth-dh2323-computer-graphics.github.io/CrowdSimulator/)

## Quick Access
* [Blog](https://github.com/KTH-DH2323-Computer-Graphics/Project/discussions)
* [Project Report]()

## Development
Here is an short guide how to work with a Unity project in GitHub.

---
### Commit Standards
Lets say that you want to add a new feature, for example, add a rotate animation and a color animation to an existing cube. The following steps could be followed:

#### **Step 1**  
If there not already exist an issue for a given feature, create one. The name of the new issue could be "Add cube animations" and a short description why this should be added.

#### **Step 2**
Create a new branch using the following command
```bash
    git checkout -b issue/{issue-number}-{arbitrarily-name}
```
The `{issue-number}` should be the number of the issue which you can see in GitHub after the issue title. The `{arbitrarily-name}` is just a name that you can give the branch to easier find it. In you case it could be: `git checkout -b issue/5-add-cube-animations`

#### **Step 3**
Code the rotation animation of the cube and commit them. And then code the color animation and to another commit. Your current branch have now two new commits.  

#### **Step 4**
Now you can create a new Pull-Request of your new branch. The naming convention of a Pull-Request should be: `#{issue-number} {type-of-change}: {title}`. For example `#5 feat: add new cube animations`. In the description of the PR, write `fix: #{issue-number}`. GitHub will then automatically link the PR with the given issue and automatically close it when the PR is merged.

---

### Create a new release of project
The repository have a GitHub Actions configured to handle the full deployment pipeline. All that you have to do is to do the following:
#### Step 1
Go to this link [Create new release](https://github.com/KTH-DH2323-Computer-Graphics/Project/releases/new).

#### Step 2
Create a tag version, for example 1.2.3. Do an extra check of the previous versions to see that you create a correct tag.  

#### Step 3
Publish the version and expect to wait about 12 minutes before you can see the changes in the website. To see the progress of the action, see this link: [GitHub Actions](https://github.com/KTH-DH2323-Computer-Graphics/Project/actions)
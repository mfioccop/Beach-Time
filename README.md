Beach-Time
==========

## Summary

BeachTime is an ASP.NET MVC web application developed for our ASU Capstone project, sponsored by Sogeti Consulting.

The application handles user registration and authentication for *Consultants*, *Account Executives*, and *Administrators* and allows users to access a variety of features:

##### Consultants
+ Manage a profile that contains information useful to *Account Executives* such as proficient programming languages, existing framework knowledge, or previous projects through a skill-tagging system
+ Upload résumés and other relevant documents that *Account Executives* can refer to and download to effectively assign new projects
 
##### Account Executives
+ View the profiles of *Consultants* currently working with a client on a project and those "on the beach"
+ Filter lists of *Consultants* based on the metadata of the skill-tagging system to narrow down potential matches for a client or project
+ Download any uploaded files from a *Consultant's* profile to pass on to a client
+ Draft and send an email to a *Consultant* to notify them of a potential client or project

##### Administrators
+ View and edit information of all users registered in the application
+ View usage statistics of the application (page visits, concurrent logins, and other useful metrics)

***
## Best Practices & Tools

#### Branching + Merging
Please follow the [Github-flow](http://scottchacon.com/2011/08/31/github-flow.html) for all branch/merge practices:

1.  Anything in the **_master_** branch is deployable
2.  To work on something new, create a *descriptively named branch* off of master (ie: new-oauth2-scopes)
3.  Commit to that branch locally and regularly push your work to the same named branch on the server
4.  When you need feedback or help, or you think the branch is ready for merging, open a **pull request**
5.  After someone else has reviewed and signed off on the feature, you can merge it into **_master_**
6.  Once it is merged and pushed to **_master_**, you can and should deploy immediately

#### Code Consistency
+ Install [StyleCop](https://stylecop.codeplex.com/) to ensure we're all following the same coding style throughout the course of the project
+ While not necessarily consistency-related, [ReSharper](https://www.jetbrains.com/student/) is extremely helpful for navigating and refactoring the codebase so it's highly recommended to download it

#### Unit Testing + Integration
+ We will be using [NUnit](http://www.nunit.org/) for unit-testing the application
+ For continuous integration testing we will be using [Jenkins](http://jenkins-ci.org/) (using a Github webhook to let Jenkins know when new code is pushed to the repository)

### Building
#### Connection Strings
+ Connection strings are stored in the connectionStrings.config file (not tracked). An example file is below:

```
<connectionStrings>
    <add name="DefaultConnection"
			providerName="System.Data.SqlClient"
			connectionString="Data Source=(localdb)\ProjectsV12;Initial Catalog=BeachTime;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False" />
</connectionStrings>
```

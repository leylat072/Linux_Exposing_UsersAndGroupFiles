This is minimal HTTP service that exposes the user and group information on
a UNIX-like system that is usually locked away in the UNIX /etc/passwd and /etc/groups files.
the paths to the passwd and groups file is configurable, defaulting to
the standard system path. If the input files are absent or malformed, the service  indicate an error.
 

# Linux_Exposing_UsersAndGroupFiles
Linux is main project and the UnitTestProject1 is unit test project;

Need to download and run .NET Core 2.1
fneed to change the passwd and groupfiles in launchsetting of Linux and unitTestProject to the path of these files in linux, for sample I 
Have created a passwd and groupfiles and set in the launch setting to path of them.
For unitTesting after changing the variables in lunchsetting of the project copy the Property folder ofin the UnitTestProject1 to
UnitTestProject1\bin\Debug\netcoreapp2.1.

  "$schema": "http://json.schemastore.org/launchsettings.json",
  "profiles": {
    "IIS Express": {
      "commandName": "Project",
      "launchBrowser": true,
      "launchUrl": "api/users",
      "environmentVariables": {
        "groupfiles": "C:\\Users\\abia1\\source\\repos\\Linux\\Linux\\group.txt",
        "passwd": "C:\\Users\\abia1\\source\\repos\\Linux\\Linux\\passwd.txt",
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    
    
    
After setting  run the project in command line  'dotnet run' in the project folder(Linux)
these lines display in cmd;
Windows DPAPI to encrypt keys at rest.
Hosting environment: Development
Content root path: C:\Users\abia1\source\repos\Linux\Linux
Now listening on: https://localhost:5001
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.


The service should provide the following methods:
http://localhost:5000//api/groups


http://localhost:5000/api/users
Return a list of all users on the system, as defined in the /etc/passwd file.

http://localhost:5000/api/users/query[?name=<nq>][&uid=<uq>][&gid=<gq>][&comment=<cq>][&home=<
hq>][&shell=<sq>]
Return a list of users matching all of the specified query fields. The bracket notation indicates that any of the
following query parameters may be supplied:
- name
- uid
- gid
- comment
- home
- shell
Only exact matches are supported.

Example Query: http://localhost:5000/api/users/query?shell=%2Fbin%2Ffalse
Example Response:
[
{“name”: “dwoodlins”, “uid”: 1001, “gid”: 1001, “comment”: “”, “home”:
“/home/dwoodlins”, “shell”: “/bin/false”}
]

http://localhost:5000/api/users/<uid>
Return a single user with <uid>. Return 404 if <uid> is not found.
Example Response:
{“name”: “dwoodlins”, “uid”: 1001, “gid”: 1001, “comment”: “”, “home”:
“/home/dwoodlins”, “shell”: “/bin/false”}

http://localhost:5000/api/users/<uid>/groups
Return all the groups for a given user.
Example Response:
[
{“name”: “docker”, “gid”: 1002, “members”: [“dwoodlins”]}
]

http://localhost:5000/api/groups

Return a list of all groups on the system, a defined by /etc/group.
Example Response:
[
{“name”: “_analyticsusers”, “gid”: 250, “members”:
[“_analyticsd’,”_networkd”,”_timed”]},
{“name”: “docker”, “gid”: 1002, “members”: []}
]

http://localhost:5000/api/groups/query[?name=<nq>][&gid=<gq>][&member=<mq1>[&member=<mq2>][&.
..]]

Return a list of groups matching all of the specified query fields. The bracket notation indicates that any of the
following query parameters may be supplied:
- name
- gid
- member (repeated)
Any group containing all the specified members should be returned, i.e. when query members are a subset of
group members.

Example Query: http://localhost:5000/api/groups/query?member=_analyticsd&member=_networkd
Example Response:
[
{“name”: “_analyticsusers”, “gid”: 250, “members”:
[“_analyticsd’,”_networkd”,”_timed”]}
]

http://localhost:5000/api/groups/<gid>
Return a single group with <gid>. Return 404 if <gid> is not found.
Example Response:
{“name”: “docker”, “gid”: 1002, “members”: [“dwoodlins”]}

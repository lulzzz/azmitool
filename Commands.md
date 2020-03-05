# Commands overview - `azmi`

Here is the overview of all `azmi` commands and their arguments.
You can run `azmi <command> --help` in your terminal to display help for any of the commands.

These are the arguments shared amongst all commands listed below.

`--identity`
Optional String argument.
Client or application ID of managed identity used to authenticate.
Example: 117dc05c-4d12-4ac2-b5f8-5e239dc8bc54

`--verbose`
Optional boolean argument.
By default commands display short, Linux style error message.
If this argument is specified, then command will produce more verbose error output.
It would actually display full dotnet error stack.

**Comments:**
Commands marked with 🚧 are only available via dev build, and their content might not correspond to this page.

# General commands

## `azmi gettoken`
Command gettoken obtains Azure authorization token for usage in other command line tools.

`--endpoint`
Optional string argument.
Endpoint against which to authenticate.
Examples: management, storage. Default 'management'

`--jwt-format`
Optional bool argument.
Print token in JSON Web Token (JWT) format.

# Storage commands

|**command**|description|
|-|-|
|**`azmi listblobs`**|Lists all blobs in container and send their names to output.|
|**`azmi getblob`**|Downloads single blob from storage account to a local file.|
|**`azmi getblobs`** 🚧|Downloads multiple blobs from container to a local directory.|
|**`azmi setblob`**|Writes single local file to a storage account blob.|
|**`azmi setblobs`** 🚧|Writes multiple local files to a storage account container.|

## Arguments overview

|**command**|source|destination|other arguments|
|-|-|-|-|
|**`listblobs`**|`--container`| *n.a.* |`--prefix`, `--exclude`|
|**`getblob`**|`--blob`|`--file`|`--if-newer`|
|**`getblobs`**|`--container`|`--directory`|`--prefix`, `--if-newer`, `--delete-on-copy`, `--exclude`|
|**`setblob`**|`--file`|`--blob` or `--container`|`--force`|
|**`setblobs`**|`--directory`|`--container`| `--force` |

All commands support arguments `--identity` and `--verbose`.

## Source and destination arguments
Arguments source and destination are mandatory.

**`--container`**
String. URL of container to which file(s) will be uploaded or downloaded from.
Example https://myaccount.blob.core.windows.net/mycontainer

**`--blob`**
String. URL of blob which will be downloaded/updated.
Example https://myaccount.blob.core.windows.net/mycontainer/myblob.txt

**`--directory`**
String. Path to a local directory to which blobs will be downloaded to or uploaded from.
Examples: `/home/myname/blobs/` or `./mydir`

**`--file`**
String. Path to local file to which content will be downloaded or from which it will be uploaded.
Examples: `/tmp/1.txt`, `./1.xml`.

## Other optional arguments

*`--if-newer`*
Bool. Download blob(s) only if a newer version exists in a container than on local file system.

*`--force`*
Bool. Forces to overwrite existing blob(s) in Azure.

*`--prefix`*
String. Filters results to return only blobs whose name begins with the specified prefix.

*`--exclude`*
String. Specifies which blob to exclude from list or download operation.

*`--delete-on-copy`*
Bool. TODO: Add description.

## Known limitations

Commands `listblobs` and `getblobs` return upto 5,000 blobs filtered on Azure API side by `--prefix`.
Filtering with `--exclude` though providing more flexibility with regex is client side filtering.
This means it operates on server filtered set which can be already topped to first 5,000 blobs.
If storage account has more than 5,000 blobs, it is required to use `--prefix`, otherwise results might be inconclusive.

# Key Vault Secret commands

|**command**|description|
|-|-|
|**`azmi getsecret`**|Downloads single secret from Azure Key Vault.|
| T.B.D. | |

## Arguments overview

|**command**|source|destination|other arguments|
|-|-|-|-|
|**`azmi getsecret`** 🚧 |`--url`| `--file` or `--stdout` or `--variable` | |
| T.B.D. | | | |
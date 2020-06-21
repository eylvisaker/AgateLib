
## `Content.build` file.

Root content folder should have a file called `content.build`.
This file is a YAML or JSON file that describes how the content pipeline should
construct content.

### Fields

* `include` - The include field will specify a set of folders who's contents will be
combined into the output.
* `output` - The output field will specify the folder name that all the content will be written to.
* `create-mgcb` - Tells the content pipeline to output a MonoGame Content Builder file with the specified filename.
* `create-credits` - Tells the content pipeline to aggregate the credits files' contents to a single file with the specified filename.
* `index` - A list of index files the content pipeline should output. These files will contain a complete list of files that matched the specified pattern.

The output field 

### Example content.build file

```
include:
- oss-viral
- oss
- cc0
- local
output: Content
create-mgcb: Content.mgcb
create-credits: credits.txt
index:
- output: Sprites/LPC/sprite.index
  filter: "*.png"
  recurse: true
```

## `Content.index` file

Each of the root content folders should have a file called `content.index`.
This file is a YAML or JSON file that describes how to process the files within
this folder.

Additional `content.index` files can also appear in any subfolder. These will
override the settings in the parent content.index file.

### Fields

- `credits` - Specifies whether or not a credits file is required for content in this folder. Allowed values are:
  - `none` - No credits file is required.
  - `warn` - Warn if no credits file is found.
  - `required` - Fail the build if no credits file is found.
- `files` - A list of file patterns and instructions on how to process each file. Each entry has the following fields:
  - `pattern` - A pattern for matching the filename. This is a regular expression which does not match files in subfolders.
  - `as` - An instruction for how to process the files. Allowed values are detailed in the next section.
- `exclude-folders` - A list of folder names to exclude from the search.

### File processing instructions. 

Each file pattern has a corresponding instruction. Here's what the various instruction values do:

- `ignore` - These files will not be included in the content processing or the output.
- `copy` - These files will be copied to the output as-is.Files which match 
- `build` - These files will be processed by the MonoGame content pipeline. You will also need to specify `importer`, `processor`, and `processorParams` values.
- `texture` - These files will be processed as textures.
- `effect` - These files will be processed as effects.

```
credits: require
files:
- pattern: .*\.pyxel
  as: ignore
- pattern: .*\.xcf
  as: ignore
exclude-folders:
- SOURCE

```
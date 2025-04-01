# Print Pull Request Action

This is a GitHub Action that prints detailed information about a pull request, including its metadata and diffs. It's written in C# and uses the Octokit library to interact with the GitHub API.

## Usage

```yaml
name: Print Pull Request Info
on:
  pull_request:
    types: [opened, synchronize, reopened]

jobs:
  print-pr:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      
      - name: Print Pull Request Info
        uses: your-username/print-pull-request@v1
        with:
          pr_number: ${{ github.event.pull_request.number }}
```

## Inputs

| Input | Description | Required |
|-------|-------------|----------|
| `pr_number` | The number of the pull request to analyze | true |

## Output

The action will print:
- Pull request metadata (title, number, state, etc.)
- Author information
- Branch information
- Statistics (commits, changed files, additions, deletions)
- Detailed information about each changed file
- The complete diff for each file

## Features

- Written in C# using .NET 7.0
- Uses Octokit for GitHub API integration
- Self-contained executable (no .NET runtime required)
- Runs on Ubuntu
- Detailed error handling and reporting

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. 
# Delta CLI

A simple dotnet cli tool for taking snapshots of the Windows registry for comparison against a baseline.

# Installation

- Requires .NET 6.0 Runtime

Using terminal:

`dotnet tool install delta-cli`

# Features

1. Builds a heirarchical model of the registry, for use with comparison tools
2. Automatically checks for differences when providing `-t snapshot` during a scan.

# Usage

delta-cli -t <scantype> (baseline, snapshot)

- By default, all scans are stored at %AppData%\.delta-cli
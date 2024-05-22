# Hash UUID (v3/v5)

_[![Build status](https://github.com/smbecker/HashUuid/actions/workflows/ci.yaml/badge.svg?branch=main)](https://github.com/smbecker/HashUuid/actions/workflows/ci.yaml)_
_[![CodeQL analysis](https://github.com/smbecker/HashUuid/actions/workflows/codeql.yaml/badge.svg?branch=main)](https://github.com/smbecker/HashUuid/actions/workflows/codeql.yaml)_

Provides support for generating [UUID v3/v5](https://datatracker.ietf.org/doc/html/rfc4122) identifiers. There are two packages available:

| Package                                                                                                                                                       | Downloads                                                                                                                                                                     | NuGet Latest |
|---------------------------------------------------------------------------------------------------------------------------------------------------------------| ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------ |
| `HashUuid`<br />Provides the ability to generate UUID v3/v5 as `Guid` instances. It has no additional dependencies.                                         | [![Nuget](https://img.shields.io/nuget/dt/HashUuid)](https://www.nuget.org/packages/HashUuid)               | [![Nuget](https://img.shields.io/nuget/v/HashUuid)](https://www.nuget.org/packages/HashUuid) |
| `HashUlid`<br />Provides the ability to generate UUID v3/v5 as `Ulid` instances with a dependency on the [Ulid](https://www.nuget.org/packages/Ulid) package. | [![Nuget](https://img.shields.io/nuget/dt/HashUlid)](https://www.nuget.org/packages/HashUlid)               | [![Nuget](https://img.shields.io/nuget/v/HashUlid)](https://www.nuget.org/packages/HashUlid) |

## Versioning

We use [SemVer](http://semver.org/) along with [MinVer](https://github.com/adamralph/minver) for versioning. For the versions available, see the [tags on this repository](https://github.com/smbecker/HashUuid/tags).

## Contributing

Contributions are welcomed and greatly appreciated. See also the list of [contributors](https://github.com/smbecker/uuid/contributors) who participated in this project. Read the [CONTRIBUTING](CONTRIBUTING.md) guide for how to participate.

Git Hooks are enabled on this repository. You will need to run `git config --local core.hooksPath .githooks/` to enable them in your environment.

## License

This project is licensed under [Apache License, Version 2.0](https://apache.org/licenses/LICENSE-2.0).

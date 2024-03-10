# Changelog

## [0.6.0](https://github.com/YoloDev/YoloDev.Sdk/compare/YoloDev.Sdk-v0.5.1...YoloDev.Sdk-v0.6.0) (2024-03-10)


### ⚠ BREAKING CHANGES

* remove netframework support
* Make RoslynAnalyzerLib SDK instead
* update dotnet version
* update sdk version and remove embedded resources support

### Features

* Make RoslynAnalyzerLib SDK instead ([4be0381](https://github.com/YoloDev/YoloDev.Sdk/commit/4be03811e55311ae36f175cfa0bcde38c537c35c))
* update dotnet version ([0981ee1](https://github.com/YoloDev/YoloDev.Sdk/commit/0981ee135bc86fc7b5c9234e207104ade3586b47))
* update sdk version and remove embedded resources support ([0ec8db9](https://github.com/YoloDev/YoloDev.Sdk/commit/0ec8db90a43c30823ae67aad3b1d39f065630d9d))


### Miscellaneous Chores

* remove netframework support ([b6ddaa3](https://github.com/YoloDev/YoloDev.Sdk/commit/b6ddaa3085ba61af0ab5579525f6b780bb3f3a92))


### Dependencies

* update dependency microsoft.build.artifacts to v4.2.0 ([#35](https://github.com/YoloDev/YoloDev.Sdk/issues/35)) ([e132c32](https://github.com/YoloDev/YoloDev.Sdk/commit/e132c32172b2c0823a2e059142db1ded6899cac6))
* update dependency microsoft.build.notargets to v3.7.56 ([#39](https://github.com/YoloDev/YoloDev.Sdk/issues/39)) ([44dc595](https://github.com/YoloDev/YoloDev.Sdk/commit/44dc595cfc4e65b7fc6f301e44418a601cd0ebe5))
* update dependency microsoft.build.traversal to v3.4.0 ([#40](https://github.com/YoloDev/YoloDev.Sdk/issues/40)) ([a590424](https://github.com/YoloDev/YoloDev.Sdk/commit/a5904246441ab4adcff503d2d54c05b918c7eaeb))
* update dependency microsoft.codeanalysis.analyzers to v3.3.4 ([#38](https://github.com/YoloDev/YoloDev.Sdk/issues/38)) ([c91ee9f](https://github.com/YoloDev/YoloDev.Sdk/commit/c91ee9f8584ccb62a01fe9bae7eefcb48c54f966))
* update dependency microsoft.codeanalysis.csharp.workspaces to v4.9.2 ([#41](https://github.com/YoloDev/YoloDev.Sdk/issues/41)) ([d96be56](https://github.com/YoloDev/YoloDev.Sdk/commit/d96be56472f36f650bb92a77d62942b3b6434aae))

## [0.5.1](https://github.com/YoloDev/YoloDev.Sdk/compare/YoloDev.Sdk-v0.5.0...YoloDev.Sdk-v0.5.1) (2022-06-07)


### Dependencies

* update dependency microsoft.build.notargets to v3.5.6 ([#33](https://github.com/YoloDev/YoloDev.Sdk/issues/33)) ([f9abb9e](https://github.com/YoloDev/YoloDev.Sdk/commit/f9abb9e906ebd56ed5e5751e2a27890e4a11ce99))

## [0.5.0](https://www.github.com/YoloDev/YoloDev.Sdk/compare/YoloDev.Sdk-v0.4.0...YoloDev.Sdk-v0.5.0) (2021-12-14)


### ⚠ BREAKING CHANGES

* enable .net analyzers and style validation

### Features

* enable .net analyzers and style validation ([68f123c](https://www.github.com/YoloDev/YoloDev.Sdk/commit/68f123c4bc7f597076e08167e4659db9da559394))

## [0.4.0](https://www.github.com/YoloDev/YoloDev.Sdk/compare/YoloDev.Sdk-v0.3.1...YoloDev.Sdk-v0.4.0) (2021-12-14)


### ⚠ BREAKING CHANGES

* this removes support for the `VersionTxtReleaseType` property, instead use `PackageReleaseChannel`.

### Features

* add `PackageReleaseChannel` property ([f6e21b0](https://www.github.com/YoloDev/YoloDev.Sdk/commit/f6e21b07fd81fc34dea6a496e8b42301d3b1c2d4))

### [0.3.1](https://www.github.com/YoloDev/YoloDev.Sdk/compare/YoloDev.Sdk-v0.3.0...YoloDev.Sdk-v0.3.1) (2021-12-13)


### Features

* fix release flow ([997b5e8](https://www.github.com/YoloDev/YoloDev.Sdk/commit/997b5e83b2cf66b5c48e5caa28b038d6ccdfa54a))

## [0.3.0](https://www.github.com/YoloDev/YoloDev.Sdk/compare/YoloDev.Sdk-v0.2.30...YoloDev.Sdk-v0.3.0) (2021-12-13)


### ⚠ BREAKING CHANGES

* This will require a modification to the project as a version.txt file is now required.

### Features

* change from GitVersion to release-please ([6eaabc2](https://www.github.com/YoloDev/YoloDev.Sdk/commit/6eaabc209058dd13c7cd261239d2a83d5143289d))

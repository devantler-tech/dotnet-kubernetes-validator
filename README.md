# ✅ .NET Kubernetes Validator

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Test](https://github.com/devantler-tech/dotnet-kubernetes-validator/actions/workflows/test.yaml/badge.svg)](https://github.com/devantler-tech/dotnet-kubernetes-validator/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/devantler-tech/dotnet-kubernetes-validator/graph/badge.svg?token=RhQPb4fE7z)](https://codecov.io/gh/devantler-tech/dotnet-kubernetes-validator)

Simple validators for client-side validation and server-side validation of Kubernetes resources.

<details>
  <summary>Show/hide folder structure</summary>

<!-- readme-tree start -->
```
.
├── .github
│   └── workflows
├── Devantler.KubernetesValidator.ClientSide.Core
├── Devantler.KubernetesValidator.ClientSide.Polaris
├── Devantler.KubernetesValidator.ClientSide.Polaris.Tests
├── Devantler.KubernetesValidator.ClientSide.Schemas
├── Devantler.KubernetesValidator.ClientSide.Schemas.Tests
│   ├── SchemaValidatorTests
│   └── assets
│       ├── k8s-invalid
│       │   ├── apps
│       │   ├── clusters
│       │   │   └── ksail-default
│       │   │       └── flux-system
│       │   └── infrastructure
│       │       └── controllers
│       └── k8s-valid
│           ├── apps
│           ├── clusters
│           │   └── ksail-default
│           │       └── flux-system
│           └── infrastructure
│               └── controllers
├── Devantler.KubernetesValidator.ClientSide.YamlSyntax
├── Devantler.KubernetesValidator.ClientSide.YamlSyntax.Tests
│   ├── YamlSyntaxValidatorTests
│   └── assets
│       ├── k8s-invalid
│       │   ├── apps
│       │   ├── clusters
│       │   │   └── ksail-default
│       │   │       └── flux-system
│       │   └── infrastructure
│       │       └── controllers
│       └── k8s-valid
│           ├── apps
│           ├── clusters
│           │   └── ksail-default
│           │       └── flux-system
│           └── infrastructure
│               └── controllers
├── Devantler.KubernetesValidator.ServerSide.Core
├── Devantler.KubernetesValidator.ServerSide.Polaris
└── Devantler.KubernetesValidator.ServerSide.Polaris.Tests

45 directories
```
<!-- readme-tree end -->

</details>

## Prerequisites

- [.NET](https://dotnet.microsoft.com/en-us/)

## 🚀 Getting Started

To get started, you can install the packages from NuGet.

```bash
dotnet add package Devantler.KubernetesValidator.ClientSide.YamlSyntax
dotnet add package Devantler.KubernetesValidator.ClientSide.Schemas
dotnet add package Devantler.KubernetesValidator.ClientSide.Polaris

dotnet add package Devantler.KubernetesValidator.ServerSide.Polaris
```

## 📝 Usage

### Client-side validation

To use the client-side validators, all you need to do is to create an instance of the validator and call the `Validate` method with the directory path to the resources you want to validate.

```csharp
using Devantler.KubernetesValidator.ClientSide.YamlSyntax;

var validator = new YamlSyntaxValidator("path/to/resources");
var isValid = validator.Validate();
```

### Server-side validation

To use the server-side validators, all you need to do is to create an instance of the validator and call the `Validate` method with the kubeconfig file path and the context name.

```csharp
using Devantler.KubernetesValidator.ServerSide.Polaris;

var validator = new PolarisValidator("path/to/kubeconfig", "context-name");
var isValid = validator.Validate();
```

# ✅ .NET Kubernetes Validator

[![License](https://img.shields.io/badge/License-Apache_2.0-blue.svg)](https://opensource.org/licenses/Apache-2.0)
[![Test](https://github.com/devantler-tech/dotnet-kubernetes-validator/actions/workflows/test.yaml/badge.svg)](https://github.com/devantler-tech/dotnet-kubernetes-validator/actions/workflows/test.yaml)
[![codecov](https://codecov.io/gh/devantler-tech/dotnet-kubernetes-validator/graph/badge.svg?token=RhQPb4fE7z)](https://codecov.io/gh/devantler-tech/dotnet-kubernetes-validator)

Simple validators for client-side validation and server-side validation of Kubernetes resources.

## Prerequisites

- [.NET](https://dotnet.microsoft.com/en-us/)

## 🚀 Getting Started

To get started, you can install the packages from NuGet.

```bash
dotnet add package DevantlerTech.KubernetesValidator.ClientSide.YamlSyntax
dotnet add package DevantlerTech.KubernetesValidator.ClientSide.Schemas
dotnet add package DevantlerTech.KubernetesValidator.ClientSide.Polaris

dotnet add package DevantlerTech.KubernetesValidator.ServerSide.Polaris
```

## 📝 Usage

### Client-side validation

To use the client-side validators, all you need to do is to create an instance of the validator and call the `Validate` method with the directory path to the resources you want to validate.

```csharp
using DevantlerTech.KubernetesValidator.ClientSide.YamlSyntax;

var validator = new YamlSyntaxValidator("path/to/resources");
var isValid = validator.Validate();
```

### Server-side validation

To use the server-side validators, all you need to do is to create an instance of the validator and call the `Validate` method with the kubeconfig file path and the context name.

```csharp
using DevantlerTech.KubernetesValidator.ServerSide.Polaris;

var validator = new PolarisValidator("path/to/kubeconfig", "context-name");
var isValid = validator.Validate();
```

# Architecture

VSlices is split by responsibility, not by technical fashion.

## VSlices

Core primitives shared by the rest of the framework.

Includes:
- errors
- literals
- monads
- base traits

## VSlices.Domain

Domain modeling primitives.

Includes:
- value objects
- repositories contracts
- domain environments
- domain runtime capabilities

## VSlices.Application

Application behavior and feature execution.

Includes:
- features
- flows
- observability
- orchestration

## VSlices.Infrastructure

Concrete technical implementations and batteries.

Infrastructure implements capabilities, but does not define the framework mental model.
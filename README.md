# Updated version - Final Sprint

## The Project: 

Our project is to create a Chess Mixed Reality Video Game to be used on Hololens. The video game will have multiplayer features. 

## Development & Testing Tools:

- The project has been created using:
    - Unity
    - Mixed Reality Toolkit 
    - C#
    - Azure

## Progress:

The project is still in progress and therefore some features are still being developed. The following features have been successfully implemented:

- Scaled down board, pieces and highlights as per the Hololens demo.
- Piece movement 
- Castling move 
- Check recognition
- When a player is in check, they now must make a move that takes themselves out of check 
- Console will print checkmate when checkmate occurs, and stalemate if a stalemate occurs

## Development & Testing:

The project us being created with Unity and MRTK. The new features for the game are being tested locally and on a Hololens when possible. 

The CI/CD pipeline is currently in development and Azure is being used to implement this. 

## Features to be implemented as per the product backlog:

- Implement CI/CD workflow
- Optimise capturing
- Integrate chess engine made by another group
- Creation of a start menu
- Pawn promotion
- Tile highlight turns red when a take is available

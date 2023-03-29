# anino_exam
This repository covers my deliverables for Anino Inc exam (2023)


- <b>Unity version used</b>
    - Unity v2023.3.14f1 (LTS)
- <b>System setup (main classes, controllers, objects)</b>
    1. Main Classes
        1. SlotMachine.cs
            - Serves as the main logic of the slot machine. Functions includes setting up reels and symbols, spinning of slot machine itself, and handling reel results.
        2. Reel.cs
            - Serves as the logic of the "Columns" of the slot machine. Each Reel or "column" has an editable symbol set to determine possible outcomes per reel.
        3. ReelRow.cs
            - Serveres as the "cells" of the slot machine. Also Serves as an helper class of SlotMachine and Reel as it houses the data of the symbol that is currently on its cell.
        4. Symbol.cs
            - Serves as the "Possible outcome" of the slot machine. It also holds specific data like SymbolID, its icon, and its payment scheme.
    2. Controllers
        1. PlayerController.cs
            - Holds all the functions about what players can do in the game including start and stoppage of spin. It also handles the UI button functions and implements rules such as when can the player spin and stop the machine and checking of player balance.
        2. UIController.cs
            - Holds all the functions of updating UI Elements. Mostly texts like current bet, current winnings, and current balance.
    
- <b>List of data sources and how to edit them
1. Symbols
    - You can add, edit, and remove all possible symbols of the slot machine game through scriptable objects in Assets/ScriptableObjects/Symbols
    - You can adjust all possible symbols on specific reels in its respective game objects in the scene view located in SlotMachineManager/ReelParent/Reel
2. Payout lines
    - You can add, edit, and remove all possible payout lines of the slot machine game through scriptable objects in Assets/ScriptableObjects/PayoutLines
    - You can adjust all payout lines that will be used in the slot machine game through the PayoutLines array of SlotMachine component of SlotMachineManager game object in the scene view.
3. Slot Machine Configuration
    - SlotMachine component of SlotmachineManager gameobject in scene view also holds configurations like spin speed and reel stoppage delay.
    
- Additional notes:
    - Scalability of system
        - With a few more tweaks, the game can easily be integrated in a server to target spin outcoms.
    - Discuss Flexibility of the system
        - I used dynamic array structure a lot and its logic can handle adding of symbols, reels, easily.
        - I kept in mind that my output shall be like a "template" where you can create many slot machine games with it by re-skinning, and different setup of reels.
    - Discuss your use of MVC in the project
        - I separated all models from the controllers. I kept on re-iterating my logic to the point where each model and controller just minds its own functions and apply as much SOLID principle as I can.
        - Models just take waits to execute its tasks, controllers waits for user input through UI(View) and implements the rules and tells when the models will execute its task.
    - What are the possible future improvements of your project
         - Back-end to control outcome of spin
         - UI Improvements
         - Algorithm optimizations
             - Less use of conditions and more use of math equations.
         - More code commenting and documentation
         - Prefab structure
             - Some items, like the reel game objects can be prefabricated to make preceding level designs easier.

# ArcadeIdle_Case
Crystal Field: Collect & Sell

Crystal Field: Collect & Sell is a fun mobile game where you gather different types of crystals that grow automatically in the field and sell them for profit. In the game, you can collect randomly generated crystals on grids by moving your character with a joystick.

Grid System:
The game utilizes a grid system to determine where the crystals will grow. You can create grids of various sizes, such as a 4x3 grid, and easily adjust them in the Inspector. Additionally, if you want to create multiple grid blocks, you can simply use an empty object. This allows for flexible grid configurations without requiring changes in the code.

Grid System

The Grid System is a feature that allows us to randomly generate grids of any desired size using GemType ScriptableObjects added to the GemType list in the GridManager class. This feature also provides the flexibility to create new grids at any time and choose their dimensions.
The Grid System also supports creating grids in the Editor instead of runtime. This allows us to dynamically create different grid configurations during game design. This provides a great deal of flexibility to diversify the game world and offer more interesting gameplay experiences.
![Screenshot 2023-06-17 070917](https://github.com/esware/ArcadeIdle_Case/assets/48649947/648d1d6f-839f-4e11-a4fd-770506707ff4)

Crystal Cultivation:
On each grid, crystals of various types begin to grow. Timing is crucial as you aim to collect these crystals. Each crystal grows within a specific time frame and becomes collectible. Once a crystal is collected, a new random crystal spawns in its place and is added to your character's stack.

![Screenshot 2023-06-17 070730](https://github.com/esware/ArcadeIdle_Case/assets/48649947/11f7d1c5-46ae-4ade-ba20-bd48c7ab9cfb)

Gem Selling System:
Your character carries the collected crystals on their back and takes them to the selling area. Upon entering the selling area, the crystals on your character's back are sold one by one from the top, earning you gold in return. However, be careful, as the selling process stops when your character leaves the selling area. Each crystal provides additional earnings based on its size in addition to the base selling price. The gold you earn can be used to improve your character and purchase in-game items.

![Screenshot 2023-06-17 070747](https://github.com/esware/ArcadeIdle_Case/assets/48649947/33b81ed9-e4d0-47dd-a910-0bd8bd351780)

Total Gem UI:
A button located in the top-right corner of the screen allows you to view the statistics of the collected crystals. When pressed, a pop-up interface appears, displaying the number of each crystal type you have collected and the profits you have gained. This dynamic display motivates you to focus on specific crystal types and helps you strategize your gameplay.

![Screenshot 2023-06-17 070759](https://github.com/esware/ArcadeIdle_Case/assets/48649947/b07c4124-c0d4-48d3-82c4-517122447bdd)

Save System:
The game automatically saves the crystal statistics and the amount of gold earned, ensuring that your progress is preserved even if you close and reopen the game. This way, you can pick up where you left off without any loss of progress.

Crystal Field: Collect & Sell awaits you with its exciting gameplay, tactical decision-making, and character progression. Are you ready to gather crystals from the field and embark on a path to wealth?


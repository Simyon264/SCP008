# SCP008

Adds SCP-008 to the game. It's pretty simple and was made in about 2 hours. If SCP-049-2 hits someone they get SCP-008. You can heal SCP-008 with a medkit (50% Chance) or SCP-500 (100% Chance). When you die with SCP-008 you turn into SCP-049-2 at your original position.

## Config

| Name          | Description                                                                                                                                                               | Default                                                                   |
|---------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------|
| IsEnabled     | If the Plugin should start or not                                                                                                                                         | true                                                                      |
| TickRate      | The time in seconds between Damage Ticks                                                                                                                                  | 2                                                                         |
| DamageAmount  | The amount of damage each infected person gets.                                                                                                                           | 1.5                                                                       |
| SpawnMessage  | The message to display when someone gets revived because of SCP-008                                                                                                       | You were revieved because of SCP-008                                      |
| InfectMessage | The message to display when someone gets infected with SCP-008                                                                                                            | You were infected with SCP-008. Use a medkit or SCP-500 to heal yourself. |
| CureMessage   | The message to display when someone cures SCP-008                                                                                                                         | You cured SCP-008.                                                        |
| Stack008      | If SCP-008 should stack. An infected person gets damage for every SCP-008 they have.  So if you get hit twice you would get 3 damage next tick (using the default config) | false                                                                     |

# Tesira DSP Essentials Plugin (c) 2021

>**NOTE : INVALID TAGS BREAK THIS RELEASE**

## License

Provided under MIT license

## Overview

> The Tesira plugin provides device control over the Biamp Tesira family of DSPs with regards
> to the most commonly used and requested attriute and control types.

## Cloning Instructions

After forking this repository into your own GitHub space, you can create a new repository using this one as the template.  Then you must install the necessary dependencies as indicated below.

## Dependencies

The [Essentials](https://github.com/PepperDash/Essentials) libraries are required. They referenced via nuget. You must have nuget.exe installed and in the `PATH` environment variable to use the following command. Nuget.exe is available at [nuget.org](https://dist.nuget.org/win-x86-commandline/latest/nuget.exe).

### Installing Dependencies

To install dependencies once nuget.exe is installed, run the following command from the root directory of your repository:
`nuget install .\packages.config -OutputDirectory .\packages -excludeVersion`.
To verify that the packages installed correctly, open the plugin solution in your repo and make sure that all references are found, then try and build it.

## Feature Notes

Version 2.0.0 now offers developers the opportunity to individually bridge each control rather than the entire DSP object.  This should lead to more flexibility in development.  In documentation below, all references to the original object type will be referred to as **Legacy**, while new object data will be referred to as **Standalone**

When utilizing a standalone object within a Eisc bridge, it is addressed utilizing first the name of the base device, followed by a pair of two hyphens, then the key of the component.  For this reason, it is exceptionally important that all keys are unique.

For a parent device with a key of ```dsp-1``` and a component with the key of ```Fader01```, the **Standalone** component key is ``dsp-1--Fader01``.

The only exception to this rule is for the Base **Standalone** object, which has a key suffix of **DeviceInfo**

>**Important to note this version of the plugin currently implements both eiscApi and EiscApiAdvanced as valid bridge types**

``` javascript
"type": "eiscApiAdvanced"
```

## Installation

Navigate to the BUILDS folder in the repository.  Place the .cplz file into the Plugins folder for Essentials and reset the application.

## Controls and Configs

### Base Device

This is data relevant to the device as a whole.  This includes directly setting presets, passing controls directly, and recalling presets by name.

If utilizing the **Standalone** object, preset control is rolled in to this component.  The key suffix for this object type is ```DeviceInfo```.  Given a base device key of ```dsp-1```, the **Standalone** key for this device would be ```dsp-1--DeviceInfo```.

#### Digitals

| Legacy Join | Standalone Join | Type (RW) | Description          |
| ----------- | --------------- | --------- | -------------------- |
| 1           | 1               | R         | Device Online        |
| NA          | 3 - 13          | W         | Direct Select Preset |

#### Serials

| Legacy Join | Standalone Join | Type (RW) | Description           |
| ----------- | --------------- | --------- | --------------------- |
| 1           | 1               | RW        | ControlPassthru       |
| 100         | 3               | W         | Select Preset By Name |
| NA          | 3 - 13          | W         | Preset Names          |

#### Config Notes

> This configuration matches a standard essentials device configuration at the base level, with only the type being different.  This may have the type ```tesira```, ```tesiraforte```, ```tesiraserver```, ```tesiradsp```, or ```tesira-dsp```*.

``` javascript
"key": "TesiraDsp-1",
    "name": "TesiraDspTesting",
    "type": "tesiraDsp",
    "group": "dsp",
    "properties": {
        "control": {
            "endOfLineString": "\n",
            "deviceReadyResponsePattern": "",
            "method": "ssh",
            "tcpSshProperties": {
                "address": "10.11.50.191",
                "port": 22,
                "autoReconnect": true,
                "AutoReconnectIntervalMs": 10000,
                "username": "default",
                "password": "default"
            }
        }
    }
}
```

***

### Level / Mute

Controls objects with the attribute type of "level" or "mute" and subscribes to them as necessary.

This component only reports the level of the audio signal relative the the adjustable range

Within the **Legacy** object, this Join map represents a control that is part of an array of controls.  Each join number = Join Map Number + fader ```bridgeIndex``` as defined by the config.

Within the **Standalone** object, this join map represents a single control as defined by the key in the bridge.

#### Digitals

| Legacy Join | Standalone Join | Type (RW) | Description     |
| ----------- | --------------- | --------- | --------------- |
| 200         | 6               | R         | Channel Visible |
| 400         | 5               | RW        | Mute Toggle     |
| 600         | 3               | RW        | Mute On         |
| 800         | 4               | RW        | Mute Off        |
| 1000        | 1               | W         | Volume Up       |
| 1200        | 2               | W         | Volume Down     |

#### Analogs

| Legacy Join | Standalone Join | Type (RW) | Description                                         |
| ----------- | --------------- | --------- | --------------------------------------------------- |
| 200         | 1               | RW        | Volume Level                                        |
| 400         | 2               | R         | Icon (0 - Level, 1 - Mic)                           |
| 600         | 3               | R         | ControlType (0 Mute/Level, 1 LevelOnly, 2 MuteOnly) |
| 800         | 4               | R         | Permissions (Pass From Config)                      |

#### Serials

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 200         | 1               | R         | Control Label (Pass From Config) |

#### Config Example

> All Level/Mute configs must be part of a dictionary called ```faderControlBlocks```.  

``` javascript
"faderControlBlocks": {
    "LevelControl01": {
        "enabled": true,
        "isMic": false,
        "hasLevel": true,
        "hasMute": true,
        "index1" : 1,
        "index2" : 0,
        "label": "Room",
        "levelInstanceTag": "ROOMVOL",
        "muteInstanceTag": "ROOMVOL",
        "unmuteOnVolChange" : true,
        "incrementAmount" : "2.0",
        "permissions" : 0,
        "bridgeIndex" : 1
    }
```

#### Config Notes

**enabled** - enables the control to be subscribed and controlled.
**label** - Passed directly across the eisc as the *Label* value.
**isMic** - drives the *icon* feedback.
**hasLevel** - in conjunction with *hasMute*, sets the *ControlType*.
**hasMute** - in conjunction with *hasStatus*, sets the *ControlType*.
**index1** - Index 1 of the control point.
**index2** - Index 2 of the control point.
**levelInstanceTag** - Instance tag of the level control.
**muteInstanceTag** - Instance tag of the mute control.
**unmuteOnVolChange** - if *true*, will unmute a muted control when the level increases.
**incrementAmount** - the value in decimals by which a mute increment or decrement command will manipulate the level.
**permissions** - Passed directly across the eisc as the *Permissions* value.
**bridgeIndex** - The index of the control on a **Legacy** object

In the provided example config object, given a base object key of ```dsp-1```, this control would have a standalone key of ```dsp-1--LevelControl01```.

***

### Switcher

Controls objects with the attribute type of "sourceSelection" and subscribes to them as necessary.

Within the **Legacy** object, this Join map represents a control that is part of an array of controls.  Each join number = Join Map Number + fader ```bridgeIndex``` as defined by the config.

Within the **Standalone** object, this join map represents a single control as defined by the key in the bridge.

#### Digitals

None

#### Analogs

| Legacy Join | Standalone Join | Type (RW) | Description      |
| ----------- | --------------- | --------- | ---------------- |
| 150         | 1               | RW        | Source Selection |

#### Serials

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 150         | 1               | R         | Control Label (Pass From Config) |

#### Config Example

> All sourceSelector and router configs must be part of a dictionary called **switcherControlBlocks**. Router configs require that the "type" be set to "router"

``` javascript
"switcherControlBlocks" : {
    "SwitcherControl01" : {
        "enabled" : true,
        "label" : "switcher01",
        "index1" : 1
        "switcherInstanceTag" : "SourceSelector1",
        "type:": "router",
        "bridgeIndex" : 1,
        "switcherInputs" : {
            "1" : {"label": "Input1" },
            "2" : {"label": "Input2" },
        },
        "switcherOutputs" : {
            "1" : { "label": "Output1" },
            "2" : { "label": "Output2" },
        }
    }
}
```

#### Config Notes

> **enabled** - enables the control to be subscribed and controlled.
**label** - Passed directly across the eisc as the *Label* value.
**index1** - Index 1 of the control point.
**switcherInstanceTag** - Instance tag of the sourceSelection control
**bridgeIndex** - The index of the control on a **Legacy** object
**switcherInputs** - This is a dictionary of labels for inputs of the switcher.  This is earmarked for future usage.  The keys **must** be integers.
**switcherOutputs** - This is a dictionary of labels for outputs of the switcher.  This is earmarked for future usage.  The keys **must** be integers.

In the provided example config object, given a base object key of ```dsp-1```, this control would have a standalone key of ```dsp-1--SwitcherControl01```.

***

### State

Controls objects with the attribute type of "state" and subscribes to them as necessary.

Within the **Legacy** object, this Join map represents a control that is part of an array of controls.  Each join number = Join Map Number + fader ```bridgeIndex``` as defined by the config.

Within the **Standalone** object, this join map represents a single control as defined by the key in the bridge.

#### Digitals

| Legacy Join | Standalone Join | Type (RW) | Description  |
| ----------- | --------------- | --------- | ------------ |
| 1300        | 1               | RW        | State Toggle |
| 1450        | 2               | RW        | State On     |
| 1600        | 3               | RW        | State Off    |

#### Analogs

None

#### Serials

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 1300        | 1               | R         | Control Label (Pass From Config) |

#### Config Example

> All state configs must be part of a dictionary called **stateControlBlocks**.  

``` javascript
"stateControlBlocks" : {
    "StateControl1" : {
        "enabled" : true,
        "label" : "State01",
        "stateInstanceTag" : "LogicState1",
        "index" : 1,
        "bridgeIndex" : 1
    }
}
```

In the provided example config object, given a base object key of ```dsp-1```, this control would have a standalone key of ```dsp-1--StateControl1```.

#### Config Notes

**enabled** - enables the control to be subscribed and controlled.
**label** - Passed directly across the eisc as the *Label* value.
**index** - Index of the control point.
**stateInstanceTag** - Instance tag of the state control
**bridgeIndex** - The index of the control on a **Legacy** object

***

### Presets

> All state configs must be part of a dictionary called **presets**.  
>
> If you intend to ONLY do direct preset calling by string, this object is NOT required to recall presets. This activity is provided by the base level device object for the Tesira DSP.  This control method is only available within the **Legacy** object.  It is provided by the **Standalone** object ```DeviceInfo```.  Thisconfig object is required regardless of control object if preset control is required.

#### Digitals

| Join | Type (RW) | Description            |
| ---- | --------- | ---------------------- |
| 100  | W         | Select Preset By Index |

#### Analogs

None

#### Serials

| Join | Type (RW) | Description          |
| ---- | --------- | -------------------- |
| 100  | R         | Preset Name by Index |

#### Config Example

``` javascript
"presets" : {
    "SomeUniqueKey": {
        "label" : "Default",
        "presetName" : "Default Levels",
        "presetId" : 1101,
        "presetIndex" : 1
        }
}
```

#### Config Notes

**label** - Passed directly across the eisc as the *Label* value.
**presetName** - the actual name of the preset as defined in biamp software
**presetID** - the ID of the preset as defined in biamp software
**presetIndex** - the index of the preset for the digital press recall

> If a `presetName` is defined, you don't need a `presetId` and vice versa.  One or the other will be fine.
> If you are utilizing the "select preset by name" methodology, no presets need be defined in config.

***

### Dialer

VoIP Controls are tested

POTS Controls are added, but as of yet untested.

DTMF is automatically managed based on current hook state.

Within the **Legacy** object, this Join map represents a control that is part of an array of controls.  Each join number = Join Map Number + (1 + (50 * (n- 1))), where n is the index of the dialer as defined by ```bridgeIndex```.

For example, Incoming Call for Line 1 would be at join 3101, while the incoming call for Line 2 would be at join 3151.

Within the **Standalone** object, this join map represents a single control as defined by the key in the bridge.

#### Digitals

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 3100 |1| R         | Incoming Call         |
| 3106 |2| W         | Answer                |
| 3107 |3| W         | End Call              |
| 3110 |4| W         | Keypad 0              |
| 3111 |5| W         | Keypad 1              |
| 3112 |6| W         | Keypad 2              |
| 3113 |7| W         | Keypad 3              |
| 3114 |8| W         | Keypad 4              |
| 3115 |9| W         | Keypad 5              |
| 3116 |10| W         | Keypad 6              |
| 3117 |11| W         | Keypad 7              |
| 3118 |12| W         | Keypad 8              |
| 3119 |13| W         | Keypad 9              |
| 3120 |14| W         | Keypad *              |
| 3121 |15| W         | Keypad #              |
| 3122 |16| W         | Keypad Clear          |
| 3123 |17| W         | Keypad Backspace      |
| 3124 |18| RW        | Dial                  |
| 3125 |19| RW        | Auto Answer On        |
| 3126 |20| RW        | Auto Answer Off       |
| 3127 |21| RW        | Auto Answer Toggle    |
| 3129 |22| RW        | On Hook               |
| 3130 |23| RW        | Off Hook              |
| 3132 |24| RW        | Do Not Disturb Toggle |
| 3133 |25| RW        | Do Not Disturb On     |
| 3134 |26| RW        | Do Not Disturb Off    |

#### Analogs

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 3100 |1| R         | CallState Value |

#### Serials

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 3100 |1| RW        | Dial String        |
| 3101 |2| RW        | Dialer Label       |
| 3102 |3| RW        | Last Number Dialed |
| 3104 |4| R         | Caller ID Number   |
| 3105 |5| R         | Caller ID Name     |

#### Config Example

> All dialer configs must be part of a dictionary called ```dialerControlBlocks```.  

``` javascript
"dialerControlBlocks" : {
    "Dialer1" : {
        "enabled" : true,
        "label" : "Dialer 01",
        "isVoip" : true,
        "dialerInstanceTag" : "Dialer1",
        "controlStatusInstanceTag" : "VoIPControlStatus1",
        "index" : 1,
        "callAppearance" : 1,
        "clearOnHangup" : true,
        "appendDtmf" : false,
        "bridgeIndex" : 1
    }
}
```

In the provided example config object, given a base object key of ```dsp-1```, this control would have a standalone key of ```dsp-1--Dialer1```.

#### Config Notes

> **enabled** - enables the control to be subscribed and controlled.
**label** - Passed directly across the eisc as the *Label* value.
**index** - Index of the line you wish to manage.
**dialerInstanceTag** - Instance tag of the dialer control
**controlStatusInstanceTag** - Instance tag of the controlStatus control
**isVoip** - sets the device type to VoIP for internal configuration.
**callAppearance** - the index of the call appearance you wish to contorl in a VoIP line.
**clearOnHangup** - if *true* will clear the *Dial String* whenever the line goes on hook.
**appendDtmf** - if **true** will append DTMF digit presses to *Dial String*
**bridgeIndex** - The index of the control on a **Legacy** object

***

### Meter

Enables metering on the sepecified meter.  
Using this is a bad idea, please avoid unless specifically requested.  

Within the **Legacy** object this Join map represents a control that is part of an array of controls.  Each join number = Join Map Number + ```bridgeIndex``` as defined by the config.

Within the **Standalone** object, this join map represents a single control as defined by the key in the bridge.

#### Digitals

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 3501 |1| RW        | Meter Toggle |

#### Analogs

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 3501 |1| R         | Meter Feedback |

#### Serials

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 3501 |1| R         | Control Label (Pass From Config) |

#### Config Example

> All state configs must be part of a dictionary called **meterControlBlocks**.  

``` javascript
"meterControlBlocks" : {
    "Meter1" : {
        "enabled" : true,
        "label" : "State01",
        "meterInstanceTag" : "Meter1",
        "index" : 1,
        "bridgeIndex" : 1
    }
}
```

In the provided example config object, given a base object key of ```dsp-1```, this control would have a standalone key of ```dsp-1--Meter1```.

#### Config Notes

**enabled** - enables the control to be subscribed and controlled.
**label** - Passed directly across the eisc as the *Label* value.
**meterInstanceTag** - Instance tag of the meter control
**index** - Index of the control point.
**bridgeIndex** - The index of the control on a **Legacy** object

### Crosspoint State Control

Provides a control point for a single crosspoint on a MatrixMixer.  

This Join map represents a control that is part of an array of controls.  Each join number = Join Map Number + State Index as defined by the config.

#### Digitals

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 2001 |1| RW        | Crosspoint Toggle |
| 2002 |2| W         | Crosspoint On     |
| 2003 |3| W         | Crosspoint Off    |

#### Analogs

None

#### Serials

| Legacy Join | Standalone Join | Type (RW) | Description                      |
| ----------- | --------------- | --------- | -------------------------------- |
| 2001 |1| R         | Control Label (Pass From Config) |

#### Config Example

> All state configs must be part of a dictionary called **crosspointStateControlBlocks**.  

``` javascript
"crosspointStateControlBlocks" : {
    "Crosspoint1" : {
        "enabled" : true,
        "label" : "Crosspoint1-2",
        "matrixInstanceTag" : "Meter1",
        "index1" : 1,
        "index2" : 2,
        "bridgeIndex" : 1
    }
}
```

In the provided example config object, given a base object key of ```dsp-1```, this control would have a standalone key of ```dsp-1--Crosspoint1```.

#### Config Notes

**enabled** - enables the control to be subscribed and controlled  
**label** - Passed directly across the eisc as the *Label* value  
**index1** - Input of the crosspoint to be controlled  
**index2** - Output of the crosspoint to be controlled  
**stateInstanceTag** - Instance tag of the meter block  
**bridgeIndex** - The index of the control on a **Legacy** object

***

## Full Example EFS Config

>This config will create an internal loopback EISC on IPID D1 for a ssh-controlled tesira.

```javascript
{
    "system": {},
    "system_url": "http://portal-QA.devcloud.pepperdash.com/templates/0f50640b-bc89-42d5-998f-81d137d3fc98#/template_summary",
    "template": {
        "devices": [
            {
                "key": "processor",
                "uid": 0,
                "type": "rmc3",
                "name": "RMC3",
                "group": "processor",
                "supportedConfigModes": [
                    "compliance",
                    "essentials"
                ],
                "supportedSystemTypes": [
                    "hudType",
                    "presType",
                    "vtcType",
                    "custom"
                ],
                "supportsCompliance": true,
                "properties": {}
            },
            {
                "key": "TesiraDsp-1",
                "name": "TesiraDspTesting",
                "type": "tesiraDsp",
                "group": "dsp",
                "properties": {
                    "control": {
                        "endOfLineString": "\n",
                        "deviceReadyResponsePattern": "",
                        "method": "ssh",
                        "tcpSshProperties": {
                            "address": "10.11.50.191",
                            "port": 22,
                            "autoReconnect": true,
                            "AutoReconnectIntervalMs": 10000,
                            "username": "default",
                            "password": "default"
                        }
                    },
                    "faderControlBlocks": {
                        "Fader1": {
                            "enabled": true,
                            "isMic": false,
                            "hasLevel": true,
                            "hasMute": true,
                            "index1" : 1,
                            "index2" : 0,
                            "label": "Room",
                            "levelInstanceTag": "ROOMVOL",
                            "muteInstanceTag": "ROOMVOL",
                            "unmuteOnVolChange" : true,
                            "incrementAmount" : "2.0",
                            "permissions" : 0,
                            "bridgeIndex" : 1

                        },
                        "Fader2": {
                            "enabled": true,
                            "isMic": false,
                            "hasLevel": true,
                            "hasMute": true,
                            "index1" : 1,
                            "index2" : 0,
                            "label": "VTC",
                            "levelInstanceTag": "VTCRXVOL",
                            "muteInstanceTag": "VTCRXVOL",
                            "unmuteOnVolChange" : true,
                            "incrementAmount" : "2.0"
                            "permissions" : 1,
                            "bridgeIndex" : 2
                        },
                        "Fader3": {
                            "enabled": true,
                            "isMic": false,
                            "hasLevel": true,
                            "hasMute": true,
                            "index1" : 1,
                            "index2" : 0,
                            "label": "ATC",
                            "levelInstanceTag": "ATCRXVOL",
                            "muteInstanceTag": "ATCRXVOL",
                            "unmuteOnVolChange" : true,
                            "incrementAmount" : "2.0"
                            "permissions" : 2,
                            "bridgeIndex" : 3
                        },
                        "Fader4": {
                            "enabled": true,
                            "isMic": false,
                            "hasLevel": true,
                            "hasMute": true,
                            "index1" : 1,
                            "index2" : 0,
                            "label": "PGM",
                            "levelInstanceTag": "PGMVOL",
                            "muteInstanceTag": "PGMVOL",
                            "unmuteOnVolChange" : true,
                            "incrementAmount" : "2.0"
                            "permissions" : 0,
                            "bridgeIndex" : 4
                        }
                    },
                    "dialerControlBlocks" : {
                        "Dialer1" : {
                            "enabled" : true,
                            "label" : "Dialer 1",
                            "isVoip" : true,
                            "dialerInstanceTag" : "Dialer1",
                            "controlStatusInstanceTag" : "VoIPControlStatus1",
                            "index" : 1,
                            "callAppearance" : 1,
                            "clearOnHangup" : true,
                            "appendDtmf" : false
                            "bridgeIndex" : 1
                        }
                    },
                    "stateControlBlocks" : {
                        "State1" : {
                            "enabled" : true,
                            "label" : "State01",
                            "stateInstanceTag" : "LogicState1",
                            "index" : 1,
                            "bridgeIndex" : 1
                        },
                        "State2" : {
                            "enabled" : true,
                            "label" : "State02",
                            "stateInstanceTag" : "LogicState1",
                            "index" : 2,
                            "bridgeIndex" : 2
                        },
                        "State3" : {
                            "enabled" : true,
                            "label" : "State02",
                            "stateInstanceTag" : "LogicState1",
                            "index" : 3,
                            "bridgeIndex" : 3
                        },
                        "State4" : {
                            "enabled" : true,
                            "label" : "State02",
                            "stateInstanceTag" : "LogicState1",
                            "index" : 4
                            "bridgeIndex" : 4
                        }
                    },
                    "switcherControlBlocks" : {
                        "switcherControl1" : {
                            "enabled" : true,
                            "label" : "switcher01",
                            "switcherInstanceTag" : "SourceSelector1",
                            "index1" : 1,
                            "bridgeIndex" : 1,
                            "switcherInputs" : {
                                "1" : {"label": "Input1" },
                                "2" : {"label": "Input2" },
                            },
                            "switcherOutputs" : {
                                "1" : {"label": "Output1" },
                                "2" : {"label": "Output2" },
                            }
                        }
                    },
                    "crosspointStateControlBlocks" : {
                        "Crosspoint1" : {
                            "enabled" : true,
                            "label" : "Crosspoint1-2",
                            "matrixInstanceTag" : "Meter1",
                            "index1" : 1,
                            "index2" : 2,
                            "bridgeIndex" : 1
                        }
                    },
                    "presets" : {
                        "1": {
                            "label" : "Default",
                            "preset" : "Default Levels"  
                        },
                        "2" : {
                            "label" : "High",
                            "Preset" : "Noise Reduction High"
                        }
                    }
                }
            },
            {
                "key": "eisc-Dsp",
                "uid": 4,
                "name": "Bridge Dsp",
                "group": "api",
                "type": "eiscApiAdvanced",
                "properties": {
                    "control": {
                        "tcpSshProperties": {
                            "address": "127.0.0.2",
                            "port": 0
                        },
                        "ipid": "D1",
                        "method": "ipidTcp"
                    },
                    "devices": [
                        {
                            "deviceKey": "TesiraDsp-1",
                            "joinStart": 1
                        }
                    ]
                }
            }
        ],
        "info": {
            "comment": "",
            "lastModifiedDate": "2017-03-06T23:14:40.290Z",
            "lastUid": 12,
            "processorType": "Rmc3",
            "requiredControlSofwareVersion": "",
            "systemType": "MPR"
        },
        "rooms": [],
        "tieLines": []
    }
}
```

## RoadMap

1. "Generic" control - so we don't have to develop new features for controls that only you will use.
2. RouterBlock Control - It's a rarely used control, but it essentially a n > n audio switcher
3. Crosspoint Control - Mute/Unmute and Increase/Decrease volume levels on the crosspoint of matrix switches.

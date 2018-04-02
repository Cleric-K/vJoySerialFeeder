# Microsoft COM Interaction

vJoySerialFeeder (vJSF) exposes COM Automation object similar to Excel or Word objects.
The ProgID of the object is `vJoySerialFeeder.1`. The CLSID is
`{abc3f69e-8a95-4f6c-975a-0a99338c2433}`. Most languages can find the VJSF object
from the so called `Running Object Table` with commands like `GetObject`,
`GetActiveObject`, etc. Creating object is also supported, but you may need
to register the ProgID in the registry with [this .reg file](COM.reg).

In any case vJSF _must_ be running. For example, in VBScript you can use:

```VBScript
Set obj = GetObject("vJoySerialFeeder.1")
```

## Object reference
The root vJSF object has the following members:

| Name  | Type | Description |
--- | --- | ---
| Mappings | Property | List of Mappings (see below) |
| DetachHandler(handler) | Method | Use this to detach a handler (see below) |

The `Mappings` property has members:

| Name  | Type | Description |
--- | --- | ---
| Count | Property | The number of Mappings currently in vJSF |
| Item(index) | Method | Gets `Mapping` object at `index`. Indexes start from 1. |

The `Mapping` object has members:

| Name  | Type | Description |
--- | --- | ---
| Input | Property | On read, returns the `Input` value of the mapping. The property is writeable. If you write to it, make sure you protect if from being overwritten by incoming serial data. See [More about Mappings](Mappings.md). |
| Output | Property | On read, returns the `Output` value of the mapping. The property is writeable. See the remark above. |
| Type | Property | The type of the mapping as string  |
| AttachInputHandler(handler) | Method | Use this to attach a handler which is called on change of the `Input` value |
| AttachOutputHandler(handler) | Method | Use this to attach a handler which is called on change of the `Output` value |
| DetachHandler(handler) | Method | Use this to detach a handler. |

## Handlers
Handlers are objects which _must_ have a method called `OnUpdate` which takes two arguments: `OnUpdate(Input, Output)`.
When handlers are attached to a mapping, their `OnUpdate` method will be called whenever values change.

You can attach the same handler object to more than one Mapping.
If you call the `DetachHandler(handler)` method _on a Mapping_ object,
the handler object will be detached _for this Mapping_. If the same handler object is attached to _other_ mappings,
they won't be detached.\
If you call `DetachHandler(handler)` method _of the root object_, the handler will be removed from _any_ Mapping the handler object is attached to.

## Examples

### VBScript

Here is an example in VBScript to clarify everything above. Run this script with `CScript.exe` in console, otherwise, if you run it with `WScript.exe` (which happens if you double click on a `.vbs` file), you'll be flooded with message boxes.

```VB
' Get the VJSF object - works without registry
Set obj = GetObject("vJoySerialFeeder.1")

' Get the Mapping objects
Set Map_1 = obj.Mappings.Item(1)
Set Map_2 = obj.Mappings.Item(2)

' logging helper
Sub Print(Input, Output)
	WScript.Echo "Input: " & Input & ", Output: " & Output
End Sub

eventCount = 0

' Define simple Handler class
Class MyHandler
	Sub OnUpdate(input, output)
		eventCount = eventCount + 1
		Print input, output
	End Sub
End Class

Set h = New MyHandler

WScript.Echo "-- Handler example --"
Map_1.AttachOutputHandler(h)
WScript.Echo "-- Handler attached to Mapping 1. Please generate some input --"

' Get 100 events
While eventCount < 100
	WScript.Sleep(10)
Wend

Map_1.DetachHandler(h)
WScript.Echo "-- Handler dettached --"

WScript.Echo
WScript.Echo "-- Manual Reading Mapping 1--"
Print Map_1.Input, Map_1.Output

WScript.Echo
WScript.Echo "-- Writing - Make sure you have Mapping 2 with channel set to 0 --"

Print Map_2.Input, Map_2.Output

Randomize
Val = 1000 + Int(Rnd*1000)
WScript.Echo "Writing " & Val & " to Input"
Map_2.Input = Val
Print Map_2.Input, Map_2.Output

Val = Rnd
WScript.Echo "Writing " & Val & " to Output"
Map_2.Output = Val
Print Map_2.Input, Map_2.Output

WScript.Echo
WScript.Echo "Mapping 1 type: " & Map_1.Type
```

### AutoHotKey

AutoHotKey is very useful tool if you need to simulate keystrokes or perform some other kind of automations.

Here is an example how you can command your mouse with vJSF and AHK. You need three mappings:
1. AxisMapping - for X
2. AxisMapping - for Y
3. ButtonMapping - for clicking

Make sure your axes work as intended or you risk to lose control of your mouse once the AHK script is run. Here is the actual script:

```ahk
; if you have registered the ProgID in the registry you can:
;obj := ComObjActive("vJoySerialFeeder.1")
; or
;obj := ComObjCreate("vJoySerialFeeder.1")

; otherwise:
obj := ComObjActive("{abc3f69e-8a95-4f6c-975a-0a99338c2433}")
; or
;obj := ComObjCreate("{abc3f69e-8a95-4f6c-975a-0a99338c2433}")


; We will use Handler to detect when the button is pressed.
; Output handler is called only when the Output changes.
; For ButtonMappings this means that the handler is called on transition
; 0 -> 1 or 1 -> 0

Class MyHandler {
	OnUpdate(input, output)
	{
		if output > 0
			MouseClick
	}
}


h := new MyHandler
obj.Mappings.Item(3).AttachOutputHandler(h)


; For mouse movement we will get better results if we do not depend
; on events, but rather if we poll the mappings ourselves on regular
; interval. This gives consistent speed of movement of the cursor.

#Persistent

MouseSpeed := 10
MapX := obj.Mappings.Item(1)
MapY := obj.Mappings.Item(2)

SetTimer, MoveTheMouse, 10
return


MoveTheMouse:
MouseMove, (MapX.Output-0.5)*MouseSpeed, (MapY.Output-0.5)*MouseSpeed, 0, R
return
```

### Internet Explorer ActiveX
In Internet Explorer you can create `ActiveXObject` but you _must_ register
the vJSF ProgID in the registry ([.reg file](COM.reg)).

```html
<!doctype html>
<script>
	var obj = new ActiveXObject("vJoySerialFeeder.1");
	var handler = {
		OnUpdate : function(input, output) {
						document.getElementById('input').value = input;
						document.getElementById('output').value = output;
					}
	};
	var attached = false;
	
	function attach() {
		if(!attached) {
			obj.Mappings.Item(1).AttachOutputHandler(handler);
			attached = true;
		}
	}
	
	function detach() {
		if(attached) {
			obj.Mappings.Item(1).DetachHandler(handler);
			attached = false;
		}
	}
	
	function set() {
		obj.Mappings.Item(2).Output = Math.random();
	}

	
</script>

<button onclick="attach()">Attach Handler to Mapping1.Output</button>
<button onclick="detach()">Detach Handler from Mapping1</button>
<button onclick="set()">Set random Output to Mapping 2</button>

<div>
	Input <input id="input">, Output <input id="output">
</div>
```
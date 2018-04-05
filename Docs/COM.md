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


### object: vJoySerialFeeder
This is the root object that you get from `GetObject()`. It has the following
members:

#### method: Mapping(index)
* `index` <integer> the index of the mapping (starts from 1)
* returns <[Mapping](#object-mapping)>

Gets a [Mapping](#object-mapping) object for the requested `index`.

#### method: DetachHandler(handler)
* `handler` \<[Handler](#handler)>

Detach the provided `handler` from any mapping that it is attached to (see below).

#### property: Failsafe \<boolean>

Tells if Failsafe mode is active. Read-only.

---

### object: Mapping

#### property: Input \<integer>
On read, returns the `Input` value of the mapping.\
The property is writable, but make sure you set the mapping's channel to zero,
otherwise your changes will soon be overwritten by serial data.
The `Output` of the Mapping is automatically updated on write.

#### property: Output \<float>
On read, returns the `Output` value of the mapping. The value depends on
mapping type (see [More about Mappings](Mappings.md)).\
The property is writable, but make sure you set the mapping's channel to zero,
otherwise your changes will soon be overwritten by serial data.
The `Input` of the Mapping is unaffected on write.

#### property: Type \<string>
The type of the mapping.

#### method: AttachHandler(handler, [type])
* `handler` \<[Handler](#handler)> The handler to attach.
* `type` \<string> Should be one of "input", "output" or "both".
   If not provided, "output" is assumed.

Use this method to attach a [Handler](#handler) object to this mapping.
The `type` argument determines on what kind of events you want your
handler to be called.
   * "input" - the handler is called whenever the `Input` of the mapping changes value.
   * "output" - the handler is called whenever the `Output` of the mapping changes value.
   * "both"- the handler is called whenever either `Input` or `Output` of the mapping changes value

#### method: DetachHandler(handler)
* `handler` \<[Handler](#handler)> The handler to detach.

The `handler` will be detached only from this mapping. If the same `handler`
object is attached to other mappings they will continue to be active.

---

### object: Handler
* method: OnUpdate(input, output)
   * `input` <integer> the current `Input` value of the mapping
   * `output` <float> the current `Output` value of the mapping
   
Called on value change.

This object must be implemented _by you_. The only requirement is the above
method accepting two argument. After attaching the handler to a mapping
the `OnUpdate()` method will be called depending on the attachment type
(`input`, `output` or `both`). No matter what the type of the attachment is
you, will always receive _both_ the current `input` and `output` of the mapping
as arguments.


## Examples

### VBScript

Here is an example in VBScript to clarify everything above. Run this script with `CScript.exe` in console, otherwise, if you run it with `WScript.exe` (which happens if you double click on a `.vbs` file), you'll be flooded with message boxes.

```VB
' Get the VJSF object - works without registry
Set obj = GetObject("vJoySerialFeeder.1")

' Get the Mapping objects
Set Map_1 = obj.Mapping(1)
Set Map_2 = obj.Mapping(2)

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
Map_1.AttachHandler h	' When the `type` is not specified, "output" is the default
WScript.Echo "-- Handler attached to Mapping 1. Please generate some input --"

' Get 100 events
While eventCount < 100
	WScript.Sleep(10)
Wend

Map_1.DetachHandler(h)
WScript.Echo "-- Handler detached --"

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

AutoHotKey is very useful tool if you need to simulate keystrokes or perform some other kind of automation.

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
obj.Mapping(3).AttachHandler(h)


; For mouse movement we will get better results if we do not depend
; on events, but rather if we poll the mappings ourselves on regular
; interval. This gives consistent speed of movement of the cursor.

#Persistent

MouseSpeed := 10
MapX := obj.Mapping(1)
MapY := obj.Mapping(2)

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
			obj.Mapping(1).AttachHandler(handler);
			attached = true;
		}
	}

	function detach() {
		if(attached) {
			obj.Mapping(1).DetachHandler(handler);
			attached = false;
		}
	}

	function set() {
		obj.Mapping(2).Output = Math.random();
	}


</script>

<button onclick="attach()">Attach Handler to Mapping1.Output</button>
<button onclick="detach()">Detach Handler from Mapping1</button>
<button onclick="set()">Set random Output to Mapping 2</button>

<div>
	Input <input id="input">, Output <input id="output">
</div>
```
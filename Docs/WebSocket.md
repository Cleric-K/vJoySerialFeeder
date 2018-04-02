# WebSocket Interaction

vJoySerialFeeder provides an integrated WebSocket server which can be used
very easily from any modern web browser. Depending on your needs, writing
a dashboard in HTML/JavaScript is probably the fastest route. The [COM](COM.md)
approach may be appropriate for more complex scenarios, but WebSocket has the
benefit that the client may be running on a different machine. (Technically COM
can also run on a different machine with DCOM, but things get more complicated
and I doubt anyone will ever need this).

> Nore: To use WebSocket you have to enable it from the `Program`>`Options` menu.

The WebSocket protocol is very simple. You send commands to vJSF and you receive
messages.

## Messages
The messages are JSON strings which can be in one of two forms:
1. Data Message
```js
{
  "mapping": 2, // index of the mapping. 1, 2... etc
  "input": 1204, // input integer value
  "output": 0.204 // output float value
}
```

2. Error Message
```js
{
	"error": "error message string"
}
```

## Commands

Commands are sent as simple string. The following commands are supported:

Command | Description
---    | ---
`get M` | Requests a Data Message for Mapping with index `M`
`set_input M V` | Sets the `Input` of mapping `M` to the integer value `V` (`Output` is calculated automatically)
`set_output M V` | Sets the `Output` of mapping `M` to the float value `V`
`sub_input M` | Subscribes to automatic Data Messages when the `Input` value of mapping `M` changes
`sub_output M` | Subscribes to automatic Data Messages when the `Output` value of mapping `M` changes
`unsub M` | Unsubscribes from any changes in mapping `M`

## Example

```html
<!doctype html>
<script>
	ws = new WebSocket('ws://localhost:40000');
	var map1, map2;

	ws.onopen = function() {
		console.log('WebSocket connection opened');

		setInterval(updateUI, 100);
	}

	ws.onclose = function() {
		console.log('WebSocket connection closed')
	}

	ws.onmessage = function(msg) {
		var data = JSON.parse(msg.data);

		// Updating the interface on every message is not a good idea.
		// In most cases updates are too frequent and trying to update the UI on
		// each of them results in high CPU usage.
		// A better approach is to keep a state in memory and update the UI with
		// a timer on regular intervals.

		if(data.error)
			console.log(data.error)
		else if(data.mapping == 1)
			map1 = data;
		else if(data.mapping == 2)
			map2 = data;
	}

	function updateUI() {
		if(map1) {
			document.getElementById('map1_input').value = map1.input
			document.getElementById('map1_output').value = map1.output;
		}
		if(map2) {
			document.getElementById('map2_input').value = map2.input
			document.getElementById('map2_output').value = map2.output;
		}
	}

	function sub() {
		ws.send('sub_output 1');
	}

	function unsub() {
		ws.send('unsub 1');
	}

	function poll() {
		ws.send('get 2');
	}

	function onMousePadMove(e) {
		var r = document.getElementById('mousepad').getBoundingClientRect(),
			mx = (e.clientX - r.left)/r.width,
			my = (e.clientY - r.top)/r.height;

		ws.send('set_output 3 ' + mx);
		ws.send('set_output 4 ' + my);
	}
</script>

<button onclick="sub()">Subscribe to Output of Mapping 1</button>
<button onclick="unsub()">Unsubscribe from Mapping 1</button>
<button onclick="poll()">Poll Mapping 2</button>

<div id="mousepad" style="display:table-cell;width:200px; height:200px; background:#cddc39; vertical-align:middle; text-align:center" onmousemove="onMousePadMove(event)">
	Move your mouse here to generate input for Mappings 3 and 4 (don't forget to set the channels to 0)
</div>

<div>Mapping 1: Input=<input id="map1_input">, Output=<input id="map1_output"></div>
<div>Mapping 2: Input=<input id="map2_input">, Output=<input id="map2_output"></div>

```
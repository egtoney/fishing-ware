# fishing-ware

# Building

```node
function handleResize() {
	const gameWidth = 640;
	const gameHeight = 360;

	const canvas = document.querySelector("#unity-canvas");

	const viewportWidth = window.innerWidth;
	const viewportHeight = window.innerHeight;

	const gameAspect = gameWidth / gameHeight;
	const viewportAspect = viewportWidth / viewportHeight;

	// console.log('v', viewportWidth, viewportHeight, viewportAspect);
	// console.log('g', gameWidth, gameHeight, gameAspect);

	if (viewportAspect > gameAspect) {
		canvas.style.width = `${viewportHeight * gameAspect}px`;
		canvas.style.height = `${viewportHeight}px`;
	} else {
		canvas.style.width = `${viewportWidth}px`;
		canvas.style.height = `${viewportWidth / gameAspect}px`;
	}
}

handleResize();
window.onresize = () => handleResize();
```
import SmarLamp as smarLamp
import asyncio
import websockets
import json

lamp = smarLamp.LampControl()

async def handleRequest(websocket, path):
    try:
        
        async for message in websocket:
            jDict = json.loads(message)
            pattern = []
             
            for pixel in jDict["Pattern"]:
                pattern.append(smarLamp.Pixel(pixel["X"],pixel["Y"],pixel["Color"]["R"],pixel["Color"]["G"],pixel["Color"]["B"]))
        
            lamp.ApplyData(smarLamp.LampData(jDict["UsePattern"], jDict["Color"]["R"], jDict["Color"]["G"], jDict["Color"]["B"], pattern, jDict["On"]))

    except Exception as e:
        print(e)

print("Started server")
asyncio.get_event_loop().run_until_complete(websockets.serve(handleRequest, '0.0.0.0', 12345))
print("Running infinte loop")
asyncio.get_event_loop().run_forever()
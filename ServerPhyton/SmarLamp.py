from sense_hat import SenseHat
import json as Json

class LampData:
    UsePattern = False
    R = 0
    G = 0
    B = 0
    Pattern = []
    On = False

    def __init__(self, usePattern, r, g, b, pattern, on):
        self.UsePattern = usePattern
        self.R = r
        self.G = g
        self.B = b
        self.Pattern = pattern
        self.On = on

class Pixel:
    X = 0
    Y = 0
    R = 0
    G = 0
    B = 0
    
    def __init__(self, X, Y, R, G, B):
        self.X = X
        self.Y = Y
        self.R = R
        self.G = G
        self.B = B

class LampControl:
    SenseHat = None

    def ApplyData(self, data):
        if not data.On:
            self.SenseHat.clear(0, 0, 0)
            return

        if data.UsePattern:
            for pixel in data.Pattern:         
                self.SenseHat.set_pixel(pixel.X, pixel.Y, pixel.R, pixel.G, pixel.B)

        else:
            self.SenseHat.clear(data.R, data.G, data.B)

    def Init(self):
        self.SenseHat = SenseHat()
    
        
    def __init__(self):
        self.Init()

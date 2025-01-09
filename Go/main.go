package main

import (
	toolstorage "MyTools/ToolStorage"
)

func main() {
	err := toolstorage.SavePNGToJPG("D:\\图片\\background.png", "D:\\图片\\background.jpg")
	if err != nil {
		print(err)
	}
}

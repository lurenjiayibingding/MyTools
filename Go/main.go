package main

import (
	toolstorage "MyTools/ToolStorage"
)

func main() {
	// err := toolstorage.SavePNGToJPG("D:\\图片\\background.png", "D:\\图片\\background.jpg")
	err := toolstorage.CreateJpgPricture(1920, 1080, 252, 252, 252, "D:\\图片\\background.jpg")
	if err != nil {
		print(err)
	}
}

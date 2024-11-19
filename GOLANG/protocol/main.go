package main

import (
	"fmt"
	"strconv"
	"strings"
)

func main() {
	// Perform GET request
	encodedMessage, err := apiGet[RequestDTO]("/api/challenges/protocol?isTest=true")
	if err != nil {
		fmt.Println("Error getting message:", err)
		return
	}

	// Decode the binary message
	decodedMessage := binaryToString(encodedMessage.ProtocolMessage)

	// Build message info
	messageInfo, err := BuildMessageInfo(decodedMessage)
	if err != nil {
		fmt.Println("Error building message:", err)
		return
	}

	// Serialize response
	responseMessage := messageInfo.Serialize()
	decodedResponseMessage := stringToBinary(responseMessage)

	// Post response
	err = apiPost("/api/challenges/protocol", ResponseDTO{Answer: decodedResponseMessage})
	if err != nil {
		fmt.Println("Error posting response:", err)
		return
	}
}

func binaryToString(binary string) string {
	if len(binary)%8 != 0 {
		panic("Binary string length must be a multiple of 8")
	}

	var text strings.Builder
	for i := 0; i < len(binary); i += 8 {
		byteString := binary[i : i+8]
		charCode, _ := strconv.ParseInt(byteString, 2, 32)
		text.WriteRune(rune(charCode))
	}

	return text.String()
}

func stringToBinary(text string) string {
	var binary strings.Builder
	for _, c := range text {
		binaryChar := fmt.Sprintf("%08b", c)
		binary.WriteString(binaryChar)
	}
	return binary.String()
}

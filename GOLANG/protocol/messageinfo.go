package main

import (
	"fmt"
	"strconv"
	"strings"
)

type MessageInfo struct {
	SignalType  string
	MessageType string
	Latitude    float64
	Longitude   float64
	Message     string
}

func (m *MessageInfo) String() string {
	return fmt.Sprintf("SignalType: %s\nMessageType: %s\nLatitude: %f\nLongitude: %f\nMessage: %s",
		m.SignalType, m.MessageType, m.Latitude, m.Longitude, m.Message)
}

func BuildMessageInfo(message string) (*MessageInfo, error) {
	parts := strings.Split(message, "#")

	if len(parts) != 6 {
		return nil, fmt.Errorf("invalid message format: incorrect number of segments")
	}

	if parts[len(parts)-1] != "END" {
		return nil, fmt.Errorf("invalid message format: does not end in END")
	}

	latitude, err := strconv.ParseFloat(parts[2], 64)
	if err != nil {
		return nil, fmt.Errorf("error parsing latitude: %v", err)
	}

	longitude, err := strconv.ParseFloat(parts[3], 64)
	if err != nil {
		return nil, fmt.Errorf("error parsing longitude: %v", err)
	}

	return &MessageInfo{
		SignalType:  parts[0],
		MessageType: parts[1],
		Latitude:    latitude,
		Longitude:   longitude,
		Message:     parts[4],
	}, nil
}

func (m *MessageInfo) Serialize() string {
	signalMeaning := m.getSignalMeaning()
	return fmt.Sprintf("ACK#MSG#1%s#1%s#%s#END",
		doubleToString(m.Latitude),
		doubleToString(m.Longitude),
		signalMeaning)
}

func doubleToString(value float64) string {
	return strconv.FormatFloat(value, 'f', -1, 64)
}

func (m *MessageInfo) getSignalMeaning() string {
	switch m.SignalType {
	case "EMG":
		switch m.MessageType {
		case "SOS":
			return "Dispatching rescue crews"
		case "PAN":
			return "Dispatching rapid-response team"
		case "FIR":
			return "Dispatching firefighting vessel"
		case "INT":
			return "Tracking unidentified craft"
		case "MED":
			return "Dispatching medical team"
		default:
			return "/"
		}
	case "MNT":
		switch m.MessageType {
		case "CHK":
			return "Dispatching inspection crew"
		case "REP":
			return "Dispatching repair crew"
		case "REF":
			return "Dispatching tanker vessel"
		case "CLN":
			return "Dispatching specialized cleaning crew"
		default:
			return "Maintenance: Unknown type"
		}
	case "INF":
		return "Asteroid trajectory noted"
	case "REQ":
		return "Dispatching emergency-supply vessel"
	default:
		return "Unknown message"
	}
}

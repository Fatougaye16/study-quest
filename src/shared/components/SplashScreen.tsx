import React from 'react';
import { View, StyleSheet, Dimensions } from 'react-native';
import Svg, { Circle, Path, G, Text as SvgText, LinearGradient, Stop, Defs, Rect } from 'react-native-svg';

const { width, height } = Dimensions.get('window');

export default function SplashScreen() {
  return (
    <View style={styles.container}>
      <Svg width={width} height={height} viewBox={`0 0 ${width} ${height}`}>
        <Defs>
          <LinearGradient id="grad1" x1="0%" y1="0%" x2="100%" y2="100%">
            <Stop offset="0%" stopColor="#0ea5e9" stopOpacity="1" />
            <Stop offset="50%" stopColor="#38bdf8" stopOpacity="1" />
            <Stop offset="100%" stopColor="#06b6d4" stopOpacity="1" />
          </LinearGradient>
          <LinearGradient id="grad2" x1="0%" y1="0%" x2="0%" y2="100%">
            <Stop offset="0%" stopColor="#fbbf24" stopOpacity="1" />
            <Stop offset="100%" stopColor="#f59e0b" stopOpacity="1" />
          </LinearGradient>
        </Defs>

        <Rect width={width} height={height} fill="url(#grad1)" />

        <Circle cx={width * 0.2} cy={height * 0.15} r="60" fill="#ffffff" opacity="0.1" />
        <Circle cx={width * 0.8} cy={height * 0.25} r="40" fill="#ffffff" opacity="0.1" />
        <Circle cx={width * 0.1} cy={height * 0.8} r="50" fill="#ffffff" opacity="0.15" />
        <Circle cx={width * 0.85} cy={height * 0.75} r="70" fill="#ffffff" opacity="0.1" />

        <G transform={`translate(${width / 2 - 80}, ${height / 2 - 120})`}>
          <Rect x="20" y="20" width="120" height="140" rx="8" fill="#ffffff" />
          <Rect x="20" y="20" width="120" height="140" rx="8" fill="none" stroke="#0c4a6e" strokeWidth="3" />
          <Path d="M 80 20 L 80 160" stroke="#0c4a6e" strokeWidth="3" />
          <Path
            d="M 50 60 L 54 72 L 66 72 L 56 80 L 60 92 L 50 84 L 40 92 L 44 80 L 34 72 L 46 72 Z"
            fill="url(#grad2)"
          />
          <Path
            d="M 110 100 L 113 109 L 122 109 L 115 115 L 118 124 L 110 118 L 102 124 L 105 115 L 98 109 L 107 109 Z"
            fill="url(#grad2)"
          />
          <Path
            d="M 100 135 L 108 145 L 125 120"
            stroke="#10b981"
            strokeWidth="5"
            fill="none"
            strokeLinecap="round"
            strokeLinejoin="round"
          />
        </G>

        <SvgText
          x={width / 2}
          y={height / 2 + 80}
          fontSize="48"
          fontWeight="bold"
          fill="#ffffff"
          textAnchor="middle"
        >
          Study Quest
        </SvgText>

        <SvgText
          x={width / 2}
          y={height / 2 + 115}
          fontSize="18"
          fill="#ffffff"
          opacity="0.9"
          textAnchor="middle"
        >
          Your Learning Adventure Awaits
        </SvgText>

        <G transform={`translate(${width / 2 - 30}, ${height * 0.85})`}>
          <Circle cx="15" cy="0" r="6" fill="#ffffff" opacity={0.9} />
          <Circle cx="35" cy="0" r="6" fill="#ffffff" opacity={0.7} />
          <Circle cx="55" cy="0" r="6" fill="#ffffff" opacity={0.5} />
        </G>
      </Svg>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#0ea5e9',
  },
});

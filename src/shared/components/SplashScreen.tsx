import React from 'react';
import { View, Text, Image, StyleSheet, Dimensions, ActivityIndicator } from 'react-native';
import Svg, { G, Polygon, Circle } from 'react-native-svg';

const { width, height } = Dimensions.get('window');

export default function SplashScreen() {
  return (
    <View style={styles.container}>
      {/* African geometric pattern overlay */}
      <View style={styles.patternContainer}>
        <Svg width={width} height={height} viewBox={`0 0 ${width} ${height}`}>
          <G opacity={0.2}>
            {Array.from({ length: 12 }).map((_, row) =>
              Array.from({ length: 8 }).map((_, col) => {
                const x = col * 55 + (row % 2 === 0 ? 0 : 27);
                const y = row * 70;
                return (
                  <G key={`${row}-${col}`}>
                    <Polygon
                      points={`${x + 20},${y} ${x + 40},${y + 20} ${x + 20},${y + 40} ${x},${y + 20}`}
                      stroke="#FFFFFF"
                      strokeWidth="3"
                      fill="none"
                    />
                    {(row + col) % 3 === 0 && (
                      <Circle cx={x + 20} cy={y + 20} r={7} fill="#FFFFFF" />
                    )}
                    {(row + col) % 2 === 0 && (
                      <Polygon
                        points={`${x + 20},${y + 8} ${x + 28},${y + 20} ${x + 20},${y + 32} ${x + 12},${y + 20}`}
                        fill="rgba(255,255,255,0.15)"
                      />
                    )}
                  </G>
                );
              })
            )}
          </G>
        </Svg>
      </View>

      {/* Logo */}
      <Image
        source={require('../../../assets/xamxam.png')}
        style={styles.logo}
        resizeMode="contain"
      />

      {/* App name */}
      <Text style={styles.title}>XamXam</Text>
      <Text style={styles.subtitle}>Yombal Gestu</Text>

      {/* Loading indicator */}
      <View style={styles.loadingContainer}>
        <ActivityIndicator size="small" color="rgba(255,255,255,0.8)" />
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#BF4A0A',
    alignItems: 'center',
    justifyContent: 'center',
  },
  patternContainer: {
    ...StyleSheet.absoluteFillObject,
  },
  logo: {
    width: 120,
    height: 120,
    marginBottom: 20,
  },
  title: {
    fontSize: 48,
    fontWeight: 'bold',
    color: '#FFFFFF',
    letterSpacing: 2,
  },
  subtitle: {
    fontSize: 18,
    fontWeight: '600',
    color: '#FFFFFF',
    marginTop: 8,
    letterSpacing: 1,
  },
  loadingContainer: {
    position: 'absolute',
    bottom: height * 0.12,
  },
});

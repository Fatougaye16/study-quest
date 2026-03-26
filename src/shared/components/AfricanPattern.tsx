import React from 'react';
import { View, StyleSheet } from 'react-native';
import Svg, { Path, G, Rect, Polygon, Circle } from 'react-native-svg';
import { useTheme } from '../theme';

interface AfricanPatternProps {
  variant?: 'header' | 'background' | 'empty-state' | 'screen-bg';
  opacity?: number;
  color?: string;
  width?: number;
  height?: number;
}

export default function AfricanPattern({
  variant = 'header',
  opacity,
  color,
  width = 400,
  height = 200,
}: AfricanPatternProps) {
  const { theme } = useTheme();
  const patternColor = color ?? theme.colors.primary;
  const patternOpacity = opacity ?? (
    variant === 'background' ? 0.18
    : variant === 'screen-bg' ? 0.06
    : variant === 'empty-state' ? 0.22
    : 0.3
  );

  if (variant === 'screen-bg') {
    // Subtle tiling pattern for full-screen backgrounds
    return (
      <View style={[StyleSheet.absoluteFill, { overflow: 'hidden' }]} pointerEvents="none">
        <Svg width={width} height={height} viewBox="0 0 400 800">
          <G opacity={patternOpacity}>
            {/* Repeating small diamond grid */}
            {Array.from({ length: 20 }).map((_, row) =>
              Array.from({ length: 10 }).map((_, col) => {
                const x = col * 44 + (row % 2 === 0 ? 0 : 22);
                const y = row * 44;
                return (
                  <G key={`${row}-${col}`}>
                    <Polygon
                      points={`${x + 16},${y} ${x + 32},${y + 16} ${x + 16},${y + 32} ${x},${y + 16}`}
                      stroke={patternColor}
                      strokeWidth="2"
                      fill="none"
                    />
                    {(row + col) % 3 === 0 && (
                      <Circle cx={x + 16} cy={y + 16} r={4} fill={patternColor} />
                    )}
                  </G>
                );
              })
            )}
          </G>
        </Svg>
      </View>
    );
  }

  if (variant === 'background') {
    return (
      <View style={[StyleSheet.absoluteFill, { overflow: 'hidden' }]} pointerEvents="none">
        <Svg width={width} height={height} viewBox="0 0 400 400">
          <G opacity={patternOpacity}>
            {/* Kente-inspired horizontal zigzag bands */}
            {[0, 80, 160, 240, 320].map((y) => (
              <G key={y}>
                <Path
                  d={`M0 ${y + 20} L20 ${y} L40 ${y + 20} L60 ${y} L80 ${y + 20} L100 ${y} L120 ${y + 20} L140 ${y} L160 ${y + 20} L180 ${y} L200 ${y + 20} L220 ${y} L240 ${y + 20} L260 ${y} L280 ${y + 20} L300 ${y} L320 ${y + 20} L340 ${y} L360 ${y + 20} L380 ${y} L400 ${y + 20}`}
                  stroke={patternColor}
                  strokeWidth="3.5"
                  fill="none"
                />
                <Path
                  d={`M0 ${y + 40} L20 ${y + 60} L40 ${y + 40} L60 ${y + 60} L80 ${y + 40} L100 ${y + 60} L120 ${y + 40} L140 ${y + 60} L160 ${y + 40} L180 ${y + 60} L200 ${y + 40} L220 ${y + 60} L240 ${y + 40} L260 ${y + 60} L280 ${y + 40} L300 ${y + 60} L320 ${y + 40} L340 ${y + 60} L360 ${y + 40} L380 ${y + 60} L400 ${y + 40}`}
                  stroke={patternColor}
                  strokeWidth="3.5"
                  fill="none"
                />
                {/* Filled diamond accents between zigzags */}
                {[40, 120, 200, 280, 360].map((x) => (
                  <Polygon
                    key={`d-${y}-${x}`}
                    points={`${x},${y + 25} ${x + 10},${y + 33} ${x},${y + 41} ${x - 10},${y + 33}`}
                    fill={patternColor}
                  />
                ))}
              </G>
            ))}
          </G>
        </Svg>
      </View>
    );
  }

  if (variant === 'empty-state') {
    return (
      <View style={[StyleSheet.absoluteFill, { overflow: 'hidden', alignItems: 'center', justifyContent: 'center' }]} pointerEvents="none">
        <Svg width={200} height={200} viewBox="0 0 200 200">
          <G opacity={patternOpacity}>
            {/* Adinkra-inspired concentric diamond */}
            <Polygon points="100,10 190,100 100,190 10,100" stroke={patternColor} strokeWidth="4" fill="none" />
            <Polygon points="100,35 165,100 100,165 35,100" stroke={patternColor} strokeWidth="3" fill="none" />
            <Polygon points="100,60 140,100 100,140 60,100" stroke={patternColor} strokeWidth="2.5" fill="none" />
            <Circle cx="100" cy="100" r="15" stroke={patternColor} strokeWidth="3" fill="none" />
            {/* Corner accents — larger */}
            <Circle cx="100" cy="10" r="8" fill={patternColor} />
            <Circle cx="190" cy="100" r="8" fill={patternColor} />
            <Circle cx="100" cy="190" r="8" fill={patternColor} />
            <Circle cx="10" cy="100" r="8" fill={patternColor} />
          </G>
        </Svg>
      </View>
    );
  }

  // Default: header variant — geometric border strip
  return (
    <View style={{ overflow: 'hidden' }} pointerEvents="none">
      <Svg width={width} height={40} viewBox="0 0 400 40">
        <G opacity={patternOpacity}>
          {/* Repeating diamond/triangle strip */}
          {Array.from({ length: 20 }).map((_, i) => (
            <G key={i}>
              <Polygon
                points={`${i * 20 + 10},4 ${i * 20 + 20},20 ${i * 20 + 10},36 ${i * 20},20`}
                stroke={patternColor}
                strokeWidth="3"
                fill={i % 2 === 0 ? patternColor : 'none'}
              />
              {i % 2 !== 0 && (
                <Circle cx={i * 20 + 10} cy={20} r={6} fill={patternColor} />
              )}
            </G>
          ))}
        </G>
      </Svg>
    </View>
  );
}

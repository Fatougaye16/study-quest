import React from 'react';
import Svg, { G, Circle, Polygon, Path, Rect } from 'react-native-svg';

interface Props {
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}

export default function WelcomeIllustration({ size = 280, primaryColor, secondaryColor, accentColor }: Props) {
  return (
    <Svg width={size} height={size} viewBox="0 0 280 280">
      {/* Outer burst ring */}
      <G opacity={0.15}>
        {Array.from({ length: 12 }).map((_, i) => {
          const angle = (i * 30 * Math.PI) / 180;
          const x1 = 140 + 90 * Math.cos(angle);
          const y1 = 140 + 90 * Math.sin(angle);
          const x2 = 140 + 130 * Math.cos(angle);
          const y2 = 140 + 130 * Math.sin(angle);
          return (
            <Path
              key={`ray-${i}`}
              d={`M${x1} ${y1} L${x2} ${y2}`}
              stroke={primaryColor}
              strokeWidth="3"
              strokeLinecap="round"
            />
          );
        })}
      </G>

      {/* Concentric diamonds — Adinkra style */}
      <G opacity={0.2}>
        <Polygon
          points="140,30 250,140 140,250 30,140"
          stroke={primaryColor}
          strokeWidth="2.5"
          fill="none"
        />
        <Polygon
          points="140,55 225,140 140,225 55,140"
          stroke={accentColor}
          strokeWidth="2"
          fill="none"
        />
        <Polygon
          points="140,80 200,140 140,200 80,140"
          stroke={secondaryColor}
          strokeWidth="2"
          fill="none"
        />
      </G>

      {/* Corner accent dots */}
      <Circle cx="140" cy="30" r="5" fill={primaryColor} opacity={0.4} />
      <Circle cx="250" cy="140" r="5" fill={primaryColor} opacity={0.4} />
      <Circle cx="140" cy="250" r="5" fill={primaryColor} opacity={0.4} />
      <Circle cx="30" cy="140" r="5" fill={primaryColor} opacity={0.4} />

      {/* Center circle — logo placeholder */}
      <Circle cx="140" cy="140" r="45" fill={primaryColor} opacity={0.12} />
      <Circle cx="140" cy="140" r="35" fill={primaryColor} opacity={0.2} />

      {/* Open book icon in center */}
      <G>
        <Path
          d="M120 130 Q130 125 140 128 Q150 125 160 130 L160 155 Q150 152 140 155 Q130 152 120 155 Z"
          fill={primaryColor}
          opacity={0.85}
        />
        <Path
          d="M140 128 L140 155"
          stroke="white"
          strokeWidth="1.5"
        />
      </G>

      {/* Orbiting small diamonds */}
      {[0, 72, 144, 216, 288].map((deg, i) => {
        const angle = (deg * Math.PI) / 180;
        const cx = 140 + 105 * Math.cos(angle);
        const cy = 140 + 105 * Math.sin(angle);
        const colors = [primaryColor, secondaryColor, accentColor, primaryColor, secondaryColor];
        return (
          <Polygon
            key={`orb-${i}`}
            points={`${cx},${cy - 6} ${cx + 6},${cy} ${cx},${cy + 6} ${cx - 6},${cy}`}
            fill={colors[i]}
            opacity={0.5}
          />
        );
      })}

      {/* Decorative arcs */}
      <Path
        d="M80 80 Q140 60 200 80"
        stroke={accentColor}
        strokeWidth="1.5"
        fill="none"
        opacity={0.25}
      />
      <Path
        d="M80 200 Q140 220 200 200"
        stroke={accentColor}
        strokeWidth="1.5"
        fill="none"
        opacity={0.25}
      />
    </Svg>
  );
}

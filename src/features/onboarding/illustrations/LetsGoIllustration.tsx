import React from 'react';
import Svg, { G, Circle, Path, Polygon, Line } from 'react-native-svg';

interface Props {
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}

export default function LetsGoIllustration({ size = 280, primaryColor, secondaryColor, accentColor }: Props) {
  return (
    <Svg width={size} height={size} viewBox="0 0 280 280">
      {/* Outer celebration burst rays */}
      <G opacity={0.12}>
        {Array.from({ length: 16 }).map((_, i) => {
          const angle = (i * 22.5 * Math.PI) / 180;
          const x1 = 140 + 70 * Math.cos(angle);
          const y1 = 140 + 70 * Math.sin(angle);
          const x2 = 140 + 125 * Math.cos(angle);
          const y2 = 140 + 125 * Math.sin(angle);
          return (
            <Line
              key={`ray-${i}`}
              x1={x1} y1={y1} x2={x2} y2={y2}
              stroke={i % 2 === 0 ? primaryColor : accentColor}
              strokeWidth="2.5"
              strokeLinecap="round"
            />
          );
        })}
      </G>

      {/* Big check circle */}
      <Circle cx="140" cy="140" r="55" fill={primaryColor} opacity={0.1} />
      <Circle cx="140" cy="140" r="45" fill={primaryColor} opacity={0.15} />
      <Circle cx="140" cy="140" r="36" fill={primaryColor} opacity={0.22} />

      {/* Checkmark */}
      <Path
        d="M118 140 L133 155 L162 125"
        stroke={primaryColor}
        strokeWidth="6"
        strokeLinecap="round"
        strokeLinejoin="round"
        fill="none"
        opacity={0.8}
      />

      {/* Confetti / celebration shapes */}
      {/* Small colored diamonds */}
      {[
        { x: 60, y: 60, color: primaryColor },
        { x: 220, y: 55, color: secondaryColor },
        { x: 45, y: 180, color: accentColor },
        { x: 235, y: 190, color: primaryColor },
        { x: 85, y: 35, color: secondaryColor },
        { x: 195, y: 240, color: accentColor },
      ].map((d, i) => (
        <Polygon
          key={`diamond-${i}`}
          points={`${d.x},${d.y - 5} ${d.x + 5},${d.y} ${d.x},${d.y + 5} ${d.x - 5},${d.y}`}
          fill={d.color}
          opacity={0.4}
        />
      ))}

      {/* Small circles */}
      {[
        { x: 180, y: 40, color: accentColor },
        { x: 40, y: 120, color: primaryColor },
        { x: 250, y: 140, color: secondaryColor },
        { x: 75, y: 230, color: primaryColor },
        { x: 210, y: 230, color: accentColor },
      ].map((c, i) => (
        <Circle
          key={`confetti-${i}`}
          cx={c.x}
          cy={c.y}
          r={3 + (i % 2)}
          fill={c.color}
          opacity={0.35}
        />
      ))}

      {/* Sparkle stars */}
      {[
        { x: 100, y: 50, color: accentColor },
        { x: 200, y: 80, color: primaryColor },
        { x: 60, y: 200, color: secondaryColor },
        { x: 230, y: 170, color: accentColor },
      ].map((s, i) => (
        <G key={`sparkle-${i}`} opacity={0.35}>
          <Path
            d={`M${s.x} ${s.y - 5} L${s.x} ${s.y + 5} M${s.x - 5} ${s.y} L${s.x + 5} ${s.y}`}
            stroke={s.color}
            strokeWidth="2"
            strokeLinecap="round"
          />
        </G>
      ))}

      {/* Curved swoosh lines */}
      <Path
        d="M50 260 Q100 245 150 260 Q200 275 250 260"
        stroke={primaryColor}
        strokeWidth="1.5"
        fill="none"
        opacity={0.12}
      />
      <Path
        d="M30 20 Q80 35 130 20 Q180 5 230 20"
        stroke={accentColor}
        strokeWidth="1.5"
        fill="none"
        opacity={0.12}
      />
    </Svg>
  );
}

import React from 'react';
import Svg, { G, Rect, Circle, Path, Line, Polygon } from 'react-native-svg';

interface Props {
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}

export default function ProgressIllustration({ size = 280, primaryColor, secondaryColor, accentColor }: Props) {
  // Bar chart data — ascending bars
  const bars = [
    { h: 40, color: primaryColor },
    { h: 65, color: secondaryColor },
    { h: 55, color: accentColor },
    { h: 85, color: primaryColor },
    { h: 75, color: secondaryColor },
    { h: 110, color: accentColor },
    { h: 130, color: primaryColor },
  ];
  const chartLeft = 50;
  const chartBottom = 210;
  const barW = 24;
  const gap = 4;

  return (
    <Svg width={size} height={size} viewBox="0 0 280 280">
      {/* Chart baseline */}
      <Line
        x1={chartLeft - 5}
        y1={chartBottom}
        x2={chartLeft + bars.length * (barW + gap) + 5}
        y2={chartBottom}
        stroke={primaryColor}
        strokeWidth="2"
        opacity={0.2}
      />

      {/* Rising bars */}
      {bars.map((bar, i) => (
        <G key={`bar-${i}`}>
          <Rect
            x={chartLeft + i * (barW + gap)}
            y={chartBottom - bar.h}
            width={barW}
            height={bar.h}
            rx="4"
            fill={bar.color}
            opacity={0.55}
          />
          {/* Shine highlight */}
          <Rect
            x={chartLeft + i * (barW + gap) + 3}
            y={chartBottom - bar.h + 3}
            width="5"
            height={bar.h * 0.3}
            rx="2.5"
            fill="white"
            opacity={0.25}
          />
        </G>
      ))}

      {/* Trend line going up */}
      <Path
        d={`M${chartLeft + barW / 2} ${chartBottom - bars[0].h - 8} ` +
          bars.map((bar, i) =>
            `L${chartLeft + i * (barW + gap) + barW / 2} ${chartBottom - bar.h - 8}`
          ).join(' ')}
        stroke={primaryColor}
        strokeWidth="2"
        fill="none"
        opacity={0.4}
        strokeLinecap="round"
        strokeLinejoin="round"
      />

      {/* XP badge */}
      <G opacity={0.7}>
        <Circle cx="230" cy="65" r="22" fill={primaryColor} opacity={0.15} />
        <Circle cx="230" cy="65" r="16" fill={primaryColor} opacity={0.25} />
        {/* "XP" text represented as shapes */}
        <Path
          d="M224 62 L228 68 M228 62 L224 68"
          stroke="white"
          strokeWidth="2"
          strokeLinecap="round"
        />
        <Path
          d="M231 62 L231 68 M231 62 L236 62 L236 65 L231 65"
          stroke="white"
          strokeWidth="2"
          strokeLinecap="round"
          strokeLinejoin="round"
          fill="none"
        />
      </G>

      {/* Flame / streak icon */}
      <G opacity={0.6}>
        <Path
          d="M55 45 Q55 30 65 25 Q60 38 68 42 Q72 35 72 28 Q80 38 78 50 Q76 58 68 60 Q58 60 55 50 Z"
          fill={accentColor}
          opacity={0.7}
        />
        {/* Inner flame */}
        <Path
          d="M62 50 Q62 42 67 40 Q65 46 70 48 Q72 44 72 40 Q76 46 74 52 Q72 56 68 56 Q63 56 62 52 Z"
          fill={primaryColor}
          opacity={0.5}
        />
      </G>

      {/* Star accent */}
      <G opacity={0.5}>
        <Path
          d="M240 160 L243 169 L253 169 L245 175 L248 185 L240 179 L232 185 L235 175 L227 169 L237 169 Z"
          fill={accentColor}
        />
      </G>

      {/* Level up arrow */}
      <G opacity={0.3}>
        <Path
          d="M245 130 L245 115 M240 120 L245 115 L250 120"
          stroke={secondaryColor}
          strokeWidth="2.5"
          strokeLinecap="round"
          strokeLinejoin="round"
          fill="none"
        />
      </G>
    </Svg>
  );
}

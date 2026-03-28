import React from 'react';
import Svg, { G, Circle, Path, Rect, Line, Polygon } from 'react-native-svg';

interface Props {
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}

export default function StudyPlanIllustration({ size = 280, primaryColor, secondaryColor, accentColor }: Props) {
  // Winding path control points
  const milestones = [
    { x: 60, y: 230 },
    { x: 100, y: 185 },
    { x: 160, y: 175 },
    { x: 130, y: 130 },
    { x: 180, y: 95 },
    { x: 140, y: 55 },
  ];

  return (
    <Svg width={size} height={size} viewBox="0 0 280 280">
      {/* Winding road path */}
      <Path
        d="M60 230 Q80 205 100 185 Q130 165 160 175 Q180 180 170 150 Q155 130 130 130 Q110 130 120 110 Q135 90 180 95 Q200 95 195 75 Q185 55 140 55"
        stroke={primaryColor}
        strokeWidth="4"
        strokeDasharray="8,6"
        fill="none"
        opacity={0.3}
      />

      {/* Milestone circles along path */}
      {milestones.map((m, i) => {
        const isCompleted = i < 3;
        const isCurrent = i === 3;
        const colors = [primaryColor, secondaryColor, accentColor, primaryColor, secondaryColor, accentColor];
        return (
          <G key={`ms-${i}`}>
            {/* Outer ring */}
            <Circle
              cx={m.x}
              cy={m.y}
              r={isCurrent ? 14 : 11}
              fill={isCompleted ? colors[i] : 'none'}
              stroke={colors[i]}
              strokeWidth={isCurrent ? 3 : 2}
              opacity={isCompleted ? 0.7 : isCurrent ? 0.8 : 0.25}
            />
            {/* Checkmark for completed */}
            {isCompleted && (
              <Path
                d={`M${m.x - 4} ${m.y} L${m.x - 1} ${m.y + 3} L${m.x + 5} ${m.y - 3}`}
                stroke="white"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
                fill="none"
              />
            )}
            {/* Pulse ring for current */}
            {isCurrent && (
              <Circle
                cx={m.x}
                cy={m.y}
                r={18}
                stroke={colors[i]}
                strokeWidth="1.5"
                fill="none"
                opacity={0.2}
              />
            )}
          </G>
        );
      })}

      {/* Start flag */}
      <G opacity={0.5}>
        <Line x1="45" y1="220" x2="45" y2="240" stroke={primaryColor} strokeWidth="2" />
        <Polygon points="45,220 62,226 45,232" fill={primaryColor} />
      </G>

      {/* Finish star at top */}
      <G opacity={0.6}>
        <Path
          d="M140 35 L143 44 L153 44 L145 50 L148 60 L140 54 L132 60 L135 50 L127 44 L137 44 Z"
          fill={accentColor}
        />
      </G>

      {/* Floating day labels */}
      {[
        { x: 215, y: 80, label: 'Day 1' },
        { x: 220, y: 130, label: 'Day 3' },
        { x: 215, y: 180, label: 'Day 7' },
      ].map((tag, i) => (
        <G key={`tag-${i}`} opacity={0.35}>
          <Rect x={tag.x - 18} y={tag.y - 8} width="36" height="16" rx="8" fill={secondaryColor} opacity={0.2} />
        </G>
      ))}

      {/* Decorative diamond accents */}
      <Polygon points="40,100 46,106 40,112 34,106" fill={accentColor} opacity={0.15} />
      <Polygon points="240,200 246,206 240,212 234,206" fill={primaryColor} opacity={0.15} />
    </Svg>
  );
}

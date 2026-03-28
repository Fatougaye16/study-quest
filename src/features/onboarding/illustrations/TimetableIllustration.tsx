import React from 'react';
import Svg, { G, Rect, Line, Circle, Text as SvgText } from 'react-native-svg';

interface Props {
  size?: number;
  primaryColor: string;
  secondaryColor: string;
  accentColor: string;
}

const DAYS = ['M', 'T', 'W', 'T', 'F'];

export default function TimetableIllustration({ size = 280, primaryColor, secondaryColor, accentColor }: Props) {
  const gridLeft = 55;
  const gridTop = 50;
  const colW = 38;
  const rowH = 32;
  const cols = 5;
  const rows = 5;

  return (
    <Svg width={size} height={size} viewBox="0 0 280 280">
      {/* Background card */}
      <Rect x="30" y="30" width="220" height="220" rx="16" fill={primaryColor} opacity={0.06} />

      {/* Calendar header bar */}
      <Rect x="40" y="35" width="200" height="28" rx="8" fill={primaryColor} opacity={0.15} />

      {/* Day labels */}
      {DAYS.map((d, i) => (
        <SvgText
          key={`day-${i}`}
          x={gridLeft + i * colW + colW / 2}
          y={54}
          fontSize="11"
          fontWeight="700"
          fill={primaryColor}
          textAnchor="middle"
          opacity={0.8}
        >
          {d}
        </SvgText>
      ))}

      {/* Grid lines */}
      <G opacity={0.12}>
        {Array.from({ length: rows + 1 }).map((_, r) => (
          <Line
            key={`hline-${r}`}
            x1={gridLeft}
            y1={gridTop + 18 + r * rowH}
            x2={gridLeft + cols * colW}
            y2={gridTop + 18 + r * rowH}
            stroke={primaryColor}
            strokeWidth="1"
          />
        ))}
        {Array.from({ length: cols + 1 }).map((_, c) => (
          <Line
            key={`vline-${c}`}
            x1={gridLeft + c * colW}
            y1={gridTop + 18}
            x2={gridLeft + c * colW}
            y2={gridTop + 18 + rows * rowH}
            stroke={primaryColor}
            strokeWidth="1"
          />
        ))}
      </G>

      {/* Colored class blocks scattered on the grid */}
      {[
        { col: 0, row: 0, color: primaryColor, h: 1.5 },
        { col: 1, row: 1, color: secondaryColor, h: 1 },
        { col: 2, row: 0, color: accentColor, h: 1 },
        { col: 3, row: 2, color: primaryColor, h: 1.5 },
        { col: 4, row: 0, color: secondaryColor, h: 1 },
        { col: 0, row: 2.5, color: secondaryColor, h: 1 },
        { col: 1, row: 3, color: accentColor, h: 1.5 },
        { col: 2, row: 2, color: primaryColor, h: 1 },
        { col: 3, row: 4, color: accentColor, h: 1 },
        { col: 4, row: 2, color: primaryColor, h: 1.5 },
      ].map((block, i) => (
        <Rect
          key={`block-${i}`}
          x={gridLeft + block.col * colW + 3}
          y={gridTop + 18 + block.row * rowH + 3}
          width={colW - 6}
          height={rowH * block.h - 6}
          rx="4"
          fill={block.color}
          opacity={0.55}
        />
      ))}

      {/* Clock icon accent */}
      <G opacity={0.3}>
        <Circle cx="42" cy="265" r="8" stroke={primaryColor} strokeWidth="1.5" fill="none" />
        <Line x1="42" y1="265" x2="42" y2="260" stroke={primaryColor} strokeWidth="1.5" />
        <Line x1="42" y1="265" x2="45" y2="267" stroke={primaryColor} strokeWidth="1.5" />
      </G>
    </Svg>
  );
}

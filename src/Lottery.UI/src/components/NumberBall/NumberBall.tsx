import { Badge, MantineColorsTuple, useMantineTheme } from "@mantine/core";
import { CSSProperties } from "react";

interface NumberBallProps {
  selected: boolean;
  disabled?: boolean;
  value: number;
  onClick?(value: number): void;
  cursor?: CSSProperties["cursor"];
  colorRange?: MantineColorsTuple;
}

export default function NumberBall({
  selected,
  disabled,
  value,
  cursor: _cursor,
  colorRange: _colorRange,
  onClick,
}: NumberBallProps) {
  const theme = useMantineTheme();
  const cursor = _cursor ?? (!disabled || selected ? "pointer" : "not-allowed");
  const colorRange =
    _colorRange ?? (selected ? theme.colors.blue : theme.colors.gray);

  return (
    <Badge
      circle
      size="xl"
      component="button"
      variant="gradient"
      styles={() => {
        return {
          label: {
            textShadow: `1px 1px 3px ${theme.black}`,
          },
          root: {
            cursor,
            boxShadow: `3px 3px 3px ${theme.black}`,
            background: `radial-gradient(circle at 30% 30%, 
                          ${colorRange[3]} 0%, 
                          ${colorRange[5]} 40%, 
                          ${colorRange[8]} 75%)`,
          },
        };
      }}
      key={value}
      onClick={() => onClick?.(value)}
    >
      {value}
    </Badge>
  );
}

import {
  Group,
  MantineColor,
  Overlay,
  Skeleton,
  useMantineTheme,
} from "@mantine/core";
import { IconTrophyFilled, IconTrophyOff } from "@tabler/icons-react";

interface TrophyProps {
  position: number;
  loading: boolean;
  disabled?: boolean;
}

function useTrophyColor(position: number) {
  const { colors, white } = useMantineTheme();
  let color: MantineColor;
  let shadowColor: MantineColor;
  switch (position) {
    case 1:
      color = colors.yellow[7];
      shadowColor = colors.orange[6];
      break;
    case 2:
      color = colors.gray[5];
      shadowColor = white;
      break;
    case 3:
      color = colors.orange[7];
      shadowColor = colors.red[9];
      break;
    default:
      color = colors.gray[7];
      shadowColor = colors.gray[3];
      break;
  }
  return { color, shadowColor } as const;
}

function Trophy({ position, disabled, loading }: TrophyProps) {
  const { color, shadowColor } = useTrophyColor(position);

  const width = 30;
  const iconProps = { size: width };

  const icon = disabled ? (
    <IconTrophyOff {...iconProps} color="gray" />
  ) : (
    <IconTrophyFilled
      {...iconProps}
      color={color}
      stroke={"black"}
      strokeWidth={5}
      style={{
        filter: `
          drop-shadow(1px 0px 0px ${shadowColor}) `,
      }}
    />
  );

  const overlay = disabled ? null : (
    <Overlay backgroundOpacity={0} c={"white"}>
      <span
        style={{
          textShadow: "1px 1px 3px black, -1px -1px 3px grey",
          cursor: "default",
          userSelect: "none",
        }}
      >
        {loading ? "" : position}
      </span>
    </Overlay>
  );

  return (
    <Skeleton visible={loading} w={width}>
      <Group justify="center" style={{ position: "relative" }}>
        {icon}
        {overlay}
      </Group>
    </Skeleton>
  );
}

export default Trophy;

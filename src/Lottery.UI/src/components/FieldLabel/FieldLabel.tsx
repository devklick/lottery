import {
  Group,
  InputLabel,
  InputLabelProps,
  InputWrapperProps,
  Skeleton,
  Tooltip,
} from "@mantine/core";
import { IconInfoCircle } from "@tabler/icons-react";

import { useColorForTheme } from "../../common/hooks/theme.hooks";

interface FieldLabelProps {
  name: string;
  description?: string;
  required?: boolean;
  loading?: boolean;
  labelProps?: InputLabelProps;
}

export default function FieldLabel({
  name,
  description,
  required,
  loading = false,
  labelProps,
}: FieldLabelProps) {
  const infoIconColor = useColorForTheme({
    dark: ["cyan", 5],
    light: ["dark", 4],
  });

  return (
    <Tooltip
      label={description}
      disabled={!description}
      position="bottom"
      events={{ focus: false, hover: true, touch: true }}
    >
      <Skeleton visible={loading}>
        <Group w="100%" justify="space-between">
          <InputLabel {...labelProps} required={required}>
            {name}
          </InputLabel>
          {description && <IconInfoCircle size={16} color={infoIconColor} />}
        </Group>
      </Skeleton>
    </Tooltip>
  );
}

FieldLabel.AsInputProps = function ({
  name,
  description,
  required,
  loading,
}: FieldLabelProps): Pick<InputWrapperProps, "label" | "labelProps"> {
  return {
    label: (
      <FieldLabel
        name={name}
        required={required}
        description={description}
        loading={loading}
      />
    ),
    labelProps: { w: "100%" },
  };
};

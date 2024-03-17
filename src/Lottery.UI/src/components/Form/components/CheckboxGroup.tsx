import {
  Group,
  GroupProps,
  Checkbox as MantineCheckbox,
  Stack,
  StackProps,
} from "@mantine/core";
import { useController } from "react-hook-form";
import { CheckboxGroupProps, WithName } from "../types";

function CheckboxGroup(props: WithName<CheckboxGroupProps>) {
  const {
    label,
    name,
    options,
    orientation = "horizontal",
    orientationProps,
    ...rest
  } = props;
  const {
    field,
    fieldState: { error: fieldError },
    formState: { defaultValues },
  } = useController({ name });

  const { onChange, ...restField } = field;

  return (
    <MantineCheckbox.Group
      id={name}
      label={label}
      onChange={(value) => onChange(value ?? defaultValues?.[name])}
      error={fieldError?.message?.toString()}
      {...rest}
      {...restField}
    >
      {/* TODO: Reduce duplication here */}
      {orientation == "horizontal" ? (
        <Group mt="xs" {...(orientationProps as GroupProps)}>
          {options.map((option, index) => {
            const { label, value, ...rest } = option;
            return (
              <MantineCheckbox
                key={`${label}-${index}`}
                label={label}
                value={value}
                {...rest}
              />
            );
          })}
        </Group>
      ) : (
        <Stack mt={"xs"} {...(orientationProps as StackProps)}>
          {options.map((option, index) => {
            const { label, value, ...rest } = option;
            return (
              <MantineCheckbox
                key={`${label}-${index}`}
                label={label}
                value={value}
                {...rest}
              />
            );
          })}
        </Stack>
      )}
    </MantineCheckbox.Group>
  );
}

export default CheckboxGroup;

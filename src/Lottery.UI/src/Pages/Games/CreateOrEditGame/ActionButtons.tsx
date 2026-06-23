import { Button, ButtonGroup, Group, useMantineTheme } from "@mantine/core";
import { useMediaQuery } from "@mantine/hooks";

interface CreateOrEditGameActionButtonsProps {
  cancelDisabled: boolean;
  onCancel(): void;
  submitDisabled: boolean;
  formId: string;
}

export default function CreateOrEditGameActionButtons({
  cancelDisabled,
  onCancel,
  submitDisabled,
  formId,
}: CreateOrEditGameActionButtonsProps) {
  const { breakpoints } = useMantineTheme();
  const sm = useMediaQuery(`(max-width: ${breakpoints.sm})`);
  const xs = useMediaQuery(`(max-width: ${breakpoints.xs})`);
  return (
    <Group w={"100%"} justify="end">
      <ButtonGroup
        orientation={xs ? "vertical" : "horizontal"}
        w={sm ? "100%" : "auto"}
      >
        <Button
          disabled={cancelDisabled}
          type="reset"
          onClick={onCancel}
          fullWidth={sm}
          variant="outline"
          color="red"
        >
          Cancel
        </Button>
        <Button
          disabled={submitDisabled}
          type="submit"
          variant="gradient"
          gradient={{ from: "blue", to: "grape", deg: 20 }}
          fullWidth={sm}
          form={formId}
        >
          Submit
        </Button>
      </ButtonGroup>
    </Group>
  );
}

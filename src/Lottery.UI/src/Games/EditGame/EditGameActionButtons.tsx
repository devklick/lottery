import { Button, Grid, Group, useMantineTheme } from "@mantine/core";
import { useMediaQuery } from "@mantine/hooks";

interface EditGameActionButtonsProps {
  cancelDisabled: boolean;
  onCancel(): void;
  submitDisabled: boolean;
  formId: string;
}

export default function EditGameActionButtons({
  cancelDisabled,
  onCancel,
  submitDisabled,
  formId,
}: EditGameActionButtonsProps) {
  const { breakpoints } = useMantineTheme();
  const fullWidth = !useMediaQuery(`(min-width: ${breakpoints.sm})`);
  return (
    <Group w={"100%"} justify="center">
      <Grid maw={"500"} w={"100%"}>
        <Grid.Col span={{ base: 12, xs: 6 }} order={{ base: 2, xs: 1 }}>
          <Group justify="start">
            <Button
              disabled={cancelDisabled}
              type="reset"
              onClick={onCancel}
              fullWidth={fullWidth}
              variant="outline"
              color="red"
            >
              Cancel
            </Button>
          </Group>
        </Grid.Col>
        <Grid.Col span={{ base: 12, xs: 6 }} order={{ base: 1, xs: 2 }}>
          <Group justify="end">
            <Button
              disabled={submitDisabled}
              type="submit"
              variant="gradient"
              gradient={{ from: "blue", to: "grape", deg: 20 }}
              fullWidth={fullWidth}
              form={formId}
            >
              Submit
            </Button>
          </Group>
        </Grid.Col>
      </Grid>
    </Group>
  );
}

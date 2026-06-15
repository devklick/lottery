import { Button, Grid, Group } from "@mantine/core";

interface EditGameActionButtonsProps {
  fullWidth: boolean;
  cancelDisabled: boolean;
  onCancel(): void;
  submitDisabled: boolean;
}

export default function EditGameActionButtons({
  fullWidth,
  cancelDisabled,
  onCancel,
  submitDisabled,
}: EditGameActionButtonsProps) {
  return (
    <>
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
          >
            Submit
          </Button>
        </Group>
      </Grid.Col>
    </>
  );
}

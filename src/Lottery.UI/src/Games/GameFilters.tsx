import {
  Badge,
  Box,
  Button,
  Collapse,
  Container,
  Divider,
  Flex,
  Grid,
  Group,
  Modal,
  MultiSelect,
  Paper,
  Select,
  Text,
  TextInput,
} from "@mantine/core";
import { useForm, zodResolver } from "@mantine/form";
import { useDisclosure } from "@mantine/hooks";
import {
  IconFilter,
  IconFilterOff,
  IconMinus,
  IconPlus,
} from "@tabler/icons-react";
import {
  LabelledGameStates,
  LabelledSortByValues,
  SearchGamesRequestFilter,
  allGameStates,
  allSortByValues,
  searchGamesRequestFilterSchema,
} from "./games.schema";
import {
  SortDirection,
  allSortDirections,
  allSortDirectionsWithLabel,
} from "../common/schemas";

interface GameFiltersProps {
  initialValues: SearchGamesRequestFilter;
  onUpdateClicked(filters: SearchGamesRequestFilter): void;
}

function GameFilters({ initialValues, onUpdateClicked }: GameFiltersProps) {
  const [opened, { toggle }] = useDisclosure(false);

  const form = useForm<SearchGamesRequestFilter>({
    validate: zodResolver(searchGamesRequestFilterSchema),
    initialValues,
  });

  const colProps = {span: {xl: 6, lg: 6, md: 6, sm: 6, xs: 12}};

  return (
    <Container>
      <Collapse in={opened}>
        <form onSubmit={form.onSubmit((data) => onUpdateClicked(data))}>
          <Grid>
            <Grid.Col key={"name"} {...colProps}>
              <TextInput
                label="Game Name"
                {...form.getInputProps("name")}
                style={{ textAlign: "left" }}
              />
            </Grid.Col>
            <Grid.Col key={"status"} {...colProps}>
              <MultiSelect
                label="Game States"
                {...form.getInputProps("gameStates")}
                style={{ textAlign: "left" }}
                data={Object.values(LabelledGameStates)}
              />
            </Grid.Col>
            <Grid.Col key="sortby" {...colProps}>
              <Select
                label="Order By"
                {...form.getInputProps("sortBy")}
                style={{ textAlign: "left" }}
                data={Object.values(LabelledSortByValues)}
              />
            </Grid.Col>
            <Grid.Col key="sortdir" {...colProps}>
              <Select
                label="Order Direction"
                {...form.getInputProps("sortDirection")}
                style={{ textAlign: "left" }}
                data={Object.values(allSortDirectionsWithLabel)}
              />
            </Grid.Col>
          </Grid>
          <Button mb={25} mt={25} type="submit">
            Update Results
          </Button>
        </form>
      </Collapse>
      <Divider
        mb={50}
        labelPosition="right"
        label={
          <Group onClick={toggle}>
            <Badge
              ml={5}
              rightSection={
                opened ? <IconMinus size={12} /> : <IconPlus size={12} />
              }
            >
              Filters
            </Badge>
          </Group>
        }
      ></Divider>
    </Container>
  );
}

export default GameFilters;

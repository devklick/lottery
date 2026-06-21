import PageSection from "../components/PageSection";

// eslint-disable-next-line @typescript-eslint/no-empty-object-type
interface YourWinsProps {}

// eslint-disable-next-line no-empty-pattern
export default function YourWins({}: YourWinsProps) {
  return (
    <PageSection
      title="Your Wins"
      subheader="Here you can find all winning entries you have placed"
      collapsable
    />
  );
}
